using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly IProductRepository _repo;

        public ProductService(IUnitOfWork unitOfWork, IFileService fileService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<IProductRepository>();
        }

        public async Task<ResponseDTO<int>> CreateAsync(ProductCreateDTO request)
        {
            bool hasProductWithName = await _repo.HasEntityAsync(request.Name);
            if (hasProductWithName) return ResponseDTO<int>.BadRequest(Errors.AlreadyHaveProductWithName);

            UploadedFile imageFile = null;
            if (request.PhotoFileId != null)
            {
                var fileResponse = await _fileService.GetFileEntityAsync(request.PhotoFileId.Value, true);
                if (!fileResponse.Success) return ResponseDTO<int>.Error(fileResponse.HttpStatusCode, fileResponse.ErrorMessage);

                imageFile = fileResponse.Response;
            }

            Product product = _mapper.Map<Product>(request);
            product.PhotoFile = imageFile;

            await _repo.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<int>(product.Id);
        }

        public ResponseDTO<ProductEnumsDTO> GetEnumValues()
        {
            var accessLevels = Enum.GetValues<ProductAccessLevel>()
                .Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"ProductAccessLevel_{e}"))).ToList();

            var types = Enum.GetValues<ProductType>()
                .Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"ProductType_{e}"))).ToList();

            ProductEnumsDTO response = new ProductEnumsDTO { 
                AccessLevels = accessLevels,
                Types = types
            };

            return new ResponseDTO<ProductEnumsDTO>(response);
        }
    }
}
