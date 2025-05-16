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

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly IProductRepository _repo;

        public ProductService(IUnitOfWork unitOfWork, IFileService fileService, IUserService userService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _userService = userService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<IProductRepository>();
        }

        public async Task<ResponseDTO<int>> CreateAsync(ProductCreateDTO request)
        {
            bool hasProductWithName = await _repo.HasEntityAsync(request.Name);
            if (hasProductWithName) return ResponseDTO<int>.BadRequest(Errors.AlreadyHaveProductWithName);

            // Checking photo file
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

        public async Task<ResponseDTO<bool>> EditAsync(ProductEditDTO request)
        {
            Product product = await _repo.GetByIdAsync(request.Id);
            if (product == null) return ResponseDTO<bool>.NotFound();

            // Admins can edit all products, employees can edit only own created products
            int currentUserId = _userIdProvider.UserId;
            var currentUserRole = await _userService.GetUserRoleAsync(currentUserId);

            if (currentUserRole != UserRole.Admin && product.CreatorId != currentUserId) 
                return ResponseDTO<bool>.Forbidden();

            // Checking photo file
            UploadedFile imageFile = null;
            if (request.PhotoFileId != null)
            {
                var fileResponse = await _fileService.GetFileEntityAsync(request.PhotoFileId.Value, true);
                if (!fileResponse.Success) return ResponseDTO<bool>.Error(fileResponse.HttpStatusCode, fileResponse.ErrorMessage);

                imageFile = fileResponse.Response;
            }

            product = _mapper.Map(request, product);
            product.PhotoFile = imageFile;

            _repo.Update(product);
            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> SetIsOverFlag(int productId, bool flag)
        {
            Product product = await _repo.GetByIdAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound();

            // Admins can access to all products, employees can access to only own created products
            int currentUserId = _userIdProvider.UserId;
            var currentUserRole = await _userService.GetUserRoleAsync(currentUserId);

            if (currentUserRole != UserRole.Admin && product.CreatorId != currentUserId)
                return ResponseDTO<bool>.Forbidden();

            product.IsOver = flag;

            _repo.Update(product);
            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public ResponseDTO<ProductEnumsDTO> GetEnumValues()
        {
            var accessLevels = Enum.GetValues<ProductAccessLevel>()
                .Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"ProductAccessLevel_{e}"))).ToList();

            var types = Enum.GetValues<ProductType>()
                .Select(e => new EnumValueDTO((byte)e, EnumValues.ResourceManager.GetString($"ProductType_{e}"))).ToList();

            ProductEnumsDTO response = new ProductEnumsDTO
            {
                AccessLevels = accessLevels,
                Types = types
            };

            return new ResponseDTO<ProductEnumsDTO>(response);
        }
    }
}
