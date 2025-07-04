using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Users;
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
        private readonly INotificationService _notificationService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly IProductRepository _repo;
        private readonly IReportRepository _reportsRepo;

        public ProductService(IUnitOfWork unitOfWork, IAccessService accessService, IFileService fileService,
            IUserService userService, INotificationService notificationService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _fileService = fileService;
            _userService = userService;
            _notificationService = notificationService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<IProductRepository>();
            _reportsRepo = _unitOfWork.GetRepository<IReportRepository>();
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

        public async Task<ResponseDTO<bool>> ChangeIsOverFlagAsync(int productId, bool isOver)
        {
            Product product = await _repo.GetByIdWithMembersAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound();

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            product.IsOver = isOver;

            _repo.Update(product);

            // Sending notification to product members about finishing testing
            if (isOver)
            {
                string notificationText = string.Format(Common.ProductTestingFinished, product.Name);
                var memberUserIds = product.ProductMembers.Select(pm => pm.MemberId).ToList();
                await _notificationService.SendToUsersAsync(memberUserIds, false, UserNotificationKind.ProductTestingFinished,
                    notificationText, LinkedEntityType.Product, product.Id);
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> InviteUserAsync(ProductUserRequestDTO request)
        {
            Product product = await _repo.GetByIdAsync(request.ProductId);
            if (product == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundProduct);

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            // Check if user already joined into product.
            // If user itself sent a join request, he will be joined,
            // otherwise we send an invite

            var prodMem = await _repo.GetProductMemberAsync(request.ProductId, request.UserId);
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

                    string notificationText = string.Format(Common.ProductJoinRequestAccepted, product.Name);
                    await _notificationService.SendToUserAsync(request.UserId, true, UserNotificationKind.ProductJoinAccepted, notificationText, LinkedEntityType.Product, product.Id);
                }
            }
            else
            {
                User user = await _userService.GetSingleUserAsync(request.UserId);
                await _repo.AddUserToProductAsync(product.Id, user.Id, ProductMemberStatus.InviteReceived);

                string notificationText = string.Format(Common.ProductInviteRequest, product.Name);
                await _notificationService.SendToUserAsync(request.UserId, true, UserNotificationKind.ProductInvitation, notificationText, LinkedEntityType.Product, product.Id);
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> KickUserAsync(ProductUserRequestDTO request)
        {
            Product product = await _repo.GetByIdAsync(request.ProductId);
            if (product == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundProduct);

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            // Check if user already joined into product.

            var prodMem = await _repo.GetProductMemberAsync(request.ProductId, request.UserId);
            if (prodMem == null) return ResponseDTO<bool>.BadRequest(Errors.UserIsNotMember);

            _repo.RemoveUserFromProduct(prodMem);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> JoinAsync(int productId)
        {
            Product product = await _repo.GetByIdAsync(productId);
            if (product == null) return ResponseDTO<bool>.NotFound(Errors.NotFoundProduct);

            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            var prodMem = await _repo.GetProductMemberAsync(productId, currentUser.Id);
            if (prodMem != null)
            {
                if (currentUser.Role == UserRole.Tester && prodMem.Status != ProductMemberStatus.InviteReceived) return ResponseDTO<bool>.BadRequest();
                prodMem.Status = ProductMemberStatus.Joined;

                _repo.UpdateProductMember(prodMem);
                await _unitOfWork.CommitAsync();
                return new ResponseDTO<bool>(true);
            }
            else
            {
                if (currentUser.Role == UserRole.Tester && product.AccessLevel == ProductAccessLevel.Secret) return ResponseDTO<bool>.Forbidden();

                User user = await _userService.GetSingleUserAsync(currentUser.Id);
                var status = currentUser.Role == UserRole.Tester && product.AccessLevel == ProductAccessLevel.Closed
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
        public async Task<PaginationResponseDTO<ProductDTO>> GetAllAsync(GetProductsRequestDTO request)
        {
            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);
            PaginationResult<Product> result = currentUser.Role switch
            {
                UserRole.Tester => await _repo.GetWithoutSecretProductsAsync(currentUser.Id, request.PageNumber, request.PageSize,
                                                request.Type, request.SearchQuery),

                _ => await _repo.GetPageWithMembersAsync(request.PageNumber, request.PageSize, request.Type, request.SearchQuery)
            };

            List<ProductDTO> productDTOs = _mapper.Map<List<ProductDTO>>(result.Items);
            return new PaginationResponseDTO<ProductDTO>(productDTOs, result.TotalCount);
        }

        public async Task<ResponseDTO<ProductDTO>> GetByIdAsync(int productId)
        {
            User currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            var product = await _repo.GetByIdWithIncludesAsync(productId);
            if (product == null) return ResponseDTO<ProductDTO>.NotFound(Errors.NotFoundProduct);

            if (currentUser.Role == UserRole.Tester && product.AccessLevel == ProductAccessLevel.Secret)
            {
                var membership = product.ProductMembers.SingleOrDefault(pm => pm.ProductId == productId && pm.MemberId == currentUser.Id);
                var accessibleStatuses = new[] { ProductMemberStatus.Joined, ProductMemberStatus.InviteReceived };

                if (membership == null || !accessibleStatuses.Contains(membership.Status))
                    return ResponseDTO<ProductDTO>.Forbidden(Errors.ForbiddenProduct);
            }

            // Reports count (total and by status)
            var membersCount = product.ProductMembers.Count;

            List<byte> openStatuses = new List<byte> {
                (byte)ReportStatus.Open, (byte)ReportStatus.Reopened
            };

            List<byte> workingStatuses = new List<byte> {
                (byte)ReportStatus.InProgress, (byte)ReportStatus.UnderReview, (byte)ReportStatus.Fixed
            };

            List<byte> fixedStatuses = new List<byte> {
                (byte)ReportStatus.Fixed, (byte)ReportStatus.ReadyForTesting, (byte)ReportStatus.Verified
            };

            var reportCounters = await _reportsRepo.GetStatusCountersByProductAsync(productId);
            int totalReportsCount = reportCounters.Where(c => c.Key == byte.MaxValue).Select(c => c.Value).Sum();
            int openReportsCount = reportCounters.Where(c => openStatuses.Contains(c.Key)).Select(c => c.Value).Sum();
            int workingReportsCount = reportCounters.Where(c => workingStatuses.Contains(c.Key)).Select(c => c.Value).Sum();
            int fixedReportsCount = reportCounters.Where(c => fixedStatuses.Contains(c.Key)).Select(c => c.Value).Sum();

            ProductDTO dto = _mapper.Map<ProductDTO>(product);
            dto.Counters = new ProductCountersDTO
            {
                Members = membersCount,
                TotalReports = totalReportsCount,
                OpenReports = openReportsCount,
                WorkingReports = workingReportsCount,
                FixedReports = fixedReportsCount
            };

            // Employee and admin things
            if ((byte)currentUser.Role <= (byte)UserRole.Employee)
            {
                dto.Creator = _mapper.Map<UserDTO>(product.Creator);
            }

            return new ResponseDTO<ProductDTO>(dto);
        }

        public async Task<PaginationResponseDTO<UserDTO>> GetMembersAsync(GetMembersRequestDTO request)
        {
            Product product = null;
            var access = await CheckAccessAsync(request.ProductId, false);
            if (!access.Success) return PaginationResponseDTO<UserDTO>.Error(access);
            product = access.Response;

            var result = await _repo.GetUsersForProductByStatusAsync(ProductMemberStatus.Joined, request.ProductId, request.PageNumber, request.PageSize);
            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(result.Items);
            return new PaginationResponseDTO<UserDTO>(userDTOs, result.TotalCount);
        }

        // Returns a list of products that user is joined to product, or have invite request, etc. (depends on status)
        public async Task<PaginationResponseDTO<ProductDTO>> GetProductsByUserMembershipAsync(int userId, ProductMemberStatus status, PaginationRequestDTO request)
        {
            var result = await _repo.GetProductsForUserByStatusAsync(status, userId, request.PageNumber, request.PageSize);
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

        // Returns a list of products that current user has joined.
        public async Task<PaginationResponseDTO<ProductDTO>> GetJoinedProductsAsync(PaginationRequestDTO request)
        {
            int currentUserId = _userIdProvider.UserId;

            var result = await GetProductsByUserMembershipAsync(currentUserId, ProductMemberStatus.Joined, request);
            return result;
        }

        // For product creator and admins: get users that sent join request to product
        public async Task<PaginationResponseDTO<UserDTO>> GetJoinRequestUsers(GetMembersRequestDTO request)
        {
            Product product = await _repo.GetByIdAsync(request.ProductId);
            if (product == null) return PaginationResponseDTO<UserDTO>.NotFound();

            // Admins can access to all products, employees can access to only own created products
            var access = await _accessService.CheckAccessAsync(product);
            if (!access.Success) return PaginationResponseDTO<UserDTO>.Error(access);

            var result = await _repo.GetUsersForProductByStatusAsync(ProductMemberStatus.JoinRequested, request.ProductId, request.PageNumber, request.PageSize);
            var users = result.Items;

            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(users);
            return new PaginationResponseDTO<UserDTO>(userDTOs, result.TotalCount);

        }

        // Users that not member of the open product can access to it, but not create reports for this.
        public async Task<ResponseDTO<Product>> CheckAccessAsync(int productId, bool toCreateReport, int userId = 0)
        {
            if (userId == 0) userId = _userIdProvider.UserId;
            if (toCreateReport)
            {
                var membership = await _repo.GetProductMemberAsync(productId, userId);
                if (membership == null || membership.Status != ProductMemberStatus.Joined)
                    return ResponseDTO<Product>.Forbidden(Errors.ForbiddenProduct);

                return new ResponseDTO<Product>(membership.Product);
            }
            else
            {
                User currentUser = await _userService.GetSingleUserAsync(userId);
                var product = await _repo.GetByIdAsync(productId);
                if (product == null) return ResponseDTO<Product>.NotFound(Errors.NotFoundProduct);

                if (currentUser.Role != UserRole.Tester || product.AccessLevel == ProductAccessLevel.Open)
                    return new ResponseDTO<Product>(product);

                var membership = await _repo.GetProductMemberAsync(productId, userId);
                if (membership == null || membership.Status != ProductMemberStatus.Joined)
                    return ResponseDTO<Product>.Forbidden(Errors.ForbiddenProduct);

                return new ResponseDTO<Product>(membership.Product);
            }
        }
    }
}
