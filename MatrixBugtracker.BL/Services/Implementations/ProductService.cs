using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessService _accessService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly IProductRepository _repo;

        public ProductService(IUnitOfWork unitOfWork, IAccessService accessService, IFileService fileService, IUserService userService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _fileService = fileService;
            _userService = userService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<IProductRepository>();
        }

        public async Task<ResponseDTO<int?>> CreateAsync(ProductCreateDTO request)
        {
            bool hasProductWithName = await _repo.HasEntityAsync(request.Name);
            if (hasProductWithName) return ResponseDTO<int?>.BadRequest(Errors.AlreadyHaveProductWithName);

            // Checking photo file
            UploadedFile imageFile = null;
            if (request.PhotoFileId != null)
            {
                var fileResponse = await _fileService.GetFileEntityAsync(request.PhotoFileId.Value, true);
                if (!fileResponse.Success) return ResponseDTO<int?>.Error(fileResponse);

                imageFile = fileResponse.Response;
            }

            Product product = _mapper.Map<Product>(request);
            product.PhotoFile = imageFile;

            await _repo.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<int?>(product.Id);
        }

        public async Task<ResponseDTO<bool>> EditAsync(ProductEditDTO request)
        {
            Product product = await _repo.GetByIdAsync(request.Id);
            if (product == null) return ResponseDTO<bool>.NotFound();

            // Admins can edit all products, employees can edit only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            // Checking photo file
            UploadedFile imageFile = null;
            if (request.PhotoFileId != null)
            {
                var fileResponse = await _fileService.GetFileEntityAsync(request.PhotoFileId.Value, true);
                if (!fileResponse.Success) return ResponseDTO<bool>.Error(fileResponse);

                imageFile = fileResponse.Response;
            }

            product = _mapper.Map(request, product);
            product.PhotoFile = imageFile;

            _repo.Update(product);
            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> SetIsOverFlagAsync(int productId, bool flag)
        {
            Product product = await _repo.GetByIdAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound();

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            product.IsOver = flag;

            _repo.Update(product);
            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> InviteUserAsync(int productId, int userId)
        {
            Product product = await _repo.GetByIdAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundProduct);

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            // Check if user already joined into product.
            // If user itself sent a join request, he will be joined,
            // otherwise we send an invite

            var prodMem = await _repo.GetProductMemberAsync(productId, userId);
            if (prodMem != null)
            {
                if (prodMem.Status != ProductMemberStatus.JoinRequested)
                {
                    string errorMessage = prodMem.Status switch
                    {
                        ProductMemberStatus.Joined => Errors.UserAlreadyMember,
                        ProductMemberStatus.InviteReceived => Errors.ProductInviteAlreadySent,
                        _ => string.Empty
                    };
                    return ResponseDTO<bool>.BadRequest(errorMessage);
                }
                else
                {
                    prodMem.Status = ProductMemberStatus.Joined;
                    _repo.UpdateProductMember(prodMem);
                }
            }
            else
            {
                User user = await _userService.GetSingleUserAsync(userId);
                await _repo.AddUserToProductAsync(product.Id, user.Id, ProductMemberStatus.InviteReceived);
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> KickUserAsync(int productId, int userId)
        {
            Product product = await _repo.GetByIdAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundProduct);

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            // Check if user already joined into product.

            var prodMem = await _repo.GetProductMemberAsync(productId, userId);
            if (prodMem == null) return ResponseDTO<bool>.BadRequest(Errors.UserIsNotMember);

            _repo.RemoveUserFromProduct(prodMem);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> JoinAsync(int productId)
        {
            Product product = await _repo.GetByIdAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundProduct);

            int currentUserId = _userIdProvider.UserId;

            var prodMem = await _repo.GetProductMemberAsync(productId, currentUserId);
            if (prodMem != null)
            {
                if (prodMem.Status != ProductMemberStatus.InviteReceived) return ResponseDTO<bool>.BadRequest();
                prodMem.Status = ProductMemberStatus.Joined;

                _repo.UpdateProductMember(prodMem);
                await _unitOfWork.CommitAsync();
                return new ResponseDTO<bool>(true);
            }
            else
            {
                if (product.AccessLevel == ProductAccessLevel.Secret) return ResponseDTO<bool>.Forbidden();

                User user = await _userService.GetSingleUserAsync(currentUserId);
                var status = product.AccessLevel == ProductAccessLevel.Closed
                    ? ProductMemberStatus.JoinRequested : ProductMemberStatus.Joined;

                await _repo.AddUserToProductAsync(product.Id, user.Id, status);
                await _unitOfWork.CommitAsync();

                return new ResponseDTO<bool>(true);
            }
        }

        public async Task<ResponseDTO<bool>> LeaveAsync(int productId)
        {
            Product product = await _repo.GetByIdAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundProduct);

            int currentUserId = _userIdProvider.UserId;

            var prodMem = await _repo.GetProductMemberAsync(productId, currentUserId);
            if (prodMem == null) return ResponseDTO<bool>.BadRequest();

            _repo.RemoveUserFromProduct(prodMem);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }

        // Returns a list of all products in the bugtracker.
        // Note: if user is tester, the list should NOT contain secret products that the tester is not a member of.
        public async Task<PaginationResponseDTO<ProductDTO>> GetAllAsync(PaginationRequestDTO request)
        {
            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);
            PaginationResult<Product> result = currentUser.Role switch
            {
                UserRole.Tester => await _repo.GetWithoutSecretProductsAsync(currentUser.Id, request.Number, request.Size),
                _ => await _repo.GetPageWithMembersAsync(request.Number, request.Size)
            };

            List<ProductDTO> productDTOs = _mapper.Map<List<ProductDTO>>(result.Items);
            return new PaginationResponseDTO<ProductDTO>(productDTOs, result.TotalCount);
        }

        // Returns a list of products that user is joined to product, or have invite request, etc. (depends on status)
        public async Task<PaginationResponseDTO<ProductDTO>> GetProductsByUserMembershipAsync(int userId, ProductMemberStatus status, PaginationRequestDTO request)
        {
            var result = await _repo.GetProductsForUserByStatusAsync(status, userId, request.Number, request.Size);
            var products = result.Items;

            List<ProductDTO> productDTOs = _mapper.Map<List<ProductDTO>>(products);
            return new PaginationResponseDTO<ProductDTO>(productDTOs, result.TotalCount);
        }

        // Returns a list of products that current user has an invite.
        public async Task<PaginationResponseDTO<ProductDTO>> GetProductsWithInviteRequestAsync(PaginationRequestDTO request)
        {
            int currentUserId = _userIdProvider.UserId;

            var result = await GetProductsByUserMembershipAsync(currentUserId, ProductMemberStatus.InviteReceived, request);
            return result;
        }

        // Search products in the bugtracker.
        // Note: if user is tester, the list should NOT contain secret products that the tester is not a member of.
        public async Task<PaginationResponseDTO<ProductDTO>> SearchAsync(PaginatedSearchRequestDTO request)
        {
            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);
            PaginationResult<Product> result = currentUser.Role switch
            {
                UserRole.Tester => await _repo.SearchWithoutSecretProductsAsync(currentUser.Id, request.Query, request.Number, request.Size),
                _ => await _repo.SearchAsync(request.Query, request.Number, request.Size)
            };

            List<ProductDTO> productDTOs = _mapper.Map<List<ProductDTO>>(result.Items);
            return new PaginationResponseDTO<ProductDTO>(productDTOs, result.TotalCount);
        }

        // For product creator and admins: get users that sent join request to product
        public async Task<PaginationResponseDTO<UserDTO>> GetJoinRequestUsers(GetJoinRequestUsersReqDTO request)
        {
            Product product = await _repo.GetByIdAsync(request.ProductId);
            if (product == null) return PaginationResponseDTO<UserDTO>.NotFound();

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return PaginationResponseDTO<UserDTO>.Error(access);

            var result = await _repo.GetUsersForProductByStatusAsync(ProductMemberStatus.JoinRequested, request.ProductId, request.Number, request.Size);
            var users = result.Items;

            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(users);
            return new PaginationResponseDTO<UserDTO>(userDTOs, result.TotalCount);

        }

        public async Task<ResponseDTO<Product>> CheckAccessAsync(int productId)
        {
            int currentUserId = _userIdProvider.UserId;
            var membership = await _repo.GetProductMemberAsync(productId, currentUserId);
            if (membership == null || membership.Status != ProductMemberStatus.Joined)
                return ResponseDTO<Product>.Forbidden(Errors.ForbiddenProduct);

            return new ResponseDTO<Product>(membership.Product);
        }

        public ResponseDTO<ProductEnumsDTO> GetEnumValues()
        {
            var accessLevels = EnumExtensions.GetTranslatedEnums<ProductAccessLevel>();

            var types = EnumExtensions.GetTranslatedEnums<ProductType>();

            ProductEnumsDTO response = new ProductEnumsDTO
            {
                AccessLevels = accessLevels,
                Types = types
            };

            return new ResponseDTO<ProductEnumsDTO>(response);
        }
    }
}
