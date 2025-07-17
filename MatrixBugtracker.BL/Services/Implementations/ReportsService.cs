using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IProductsService _productService;
        private readonly ITagsService _tagsService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportsService> _logger;

        private readonly IReportRepository _repo;

        public ReportsService(IUnitOfWork unitOfWork, IFileService fileService,
            IProductsService productService, ITagsService tagsService,
            IUserService userService, IUserIdProvider userIdProvider,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, ILogger<ReportsService> logger)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _productService = productService;
            _tagsService = tagsService;
            _userService = userService;
            _userIdProvider = userIdProvider;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = logger;

            _repo = _unitOfWork.GetRepository<IReportRepository>();
        }

        public async Task<ResponseDTO<Report>> CheckAccessAsync(int reportId, bool needIncludes = false)
        {
            Report report = needIncludes ? await _repo.GetByIdWithIncludesAsync(reportId) : await _repo.GetByIdAsync(reportId);
            if (report == null) return ResponseDTO<Report>.NotFound();

            // We don't use AccessService.CheckAccess, because moders and employees can access every report.
            User currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            // Check if authorized user can access to vulnerability report
            // If user is tester, he can access to only vulnerabilities that created by own

            if (report.Severity == ReportSeverity.Vulnerability && currentUser.Role == UserRole.Tester)
            {
                if (report.CreatorId != currentUser.Id) return ResponseDTO<Report>.Forbidden();
            }

            // If tester (creator) has been excluded from non-open product that this report created for,
            // he can not access to this report.
            // TODO: ThenInclude ProductMembers when getting a report to optimize

            int productId = report.ProductId;
            var access = await _productService.CheckAccessAsync(productId, false);
            if (!access.Success) return ResponseDTO<Report>.Error(access);
            return new ResponseDTO<Report>(report);
        }

        public async Task<ResponseDTO<int?>> CreateAsync(ReportCreateDTO request)
        {
            Product product = null;
            var access = await _productService.CheckAccessAsync(request.ProductId, true);
            if (!access.Success) return ResponseDTO<int?>.Error(access);
            product = access.Data;

            if (product.IsOver) return ResponseDTO<int?>.BadRequest(Errors.ProductTestingIsOver);

            // Checking tags
            List<Tag> tags = null;
            if (request.Tags?.Length > 0)
            {
                var tagsCheck = await _tagsService.CheckIsAllContainsAsync(request.Tags);
                if (!tagsCheck.Success) return ResponseDTO<int?>.Error(tagsCheck);
                tags = tagsCheck.Data;
            }

            // Checking files
            List<UploadedFile> files = null;
            if (request.FileIds?.Length > 0)
            {
                if (request.FileIds.Count() > 5) return ResponseDTO<int?>.BadRequest(Errors.TooManyFiles);

                var filesCheck = await _fileService.CheckFilesAccessAsync(request.FileIds);
                if (!filesCheck.Success) return ResponseDTO<int?>.Error(filesCheck);
                files = filesCheck.Data;
            }

            Report report = _mapper.Map<Report>(request);
            report.IsAttachmentsPrivate = request.IsFilesPrivate;
            await _repo.AddAsync(report);

            if (tags?.Count > 0) await _repo.AddTagsAsync(report, tags);
            if (files?.Count > 0) await _repo.AddAttachmentAsync(report, files);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<int?>(report.Id);
        }

        public async Task<ResponseDTO<bool>> EditAsync(ReportEditDTO request)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(request.Id);
            if (!accessCheck.Success) return ResponseDTO<bool>.Error(accessCheck);
            report = accessCheck.Data;

            // We don't allow to edit reports whose status already changed or if severity changed by moderator
            // TODO: only tester (creator) can not edit, moders and higher can.
            if (report.Status != 0 || report.IsSeveritySetByModerator) return ResponseDTO<bool>.BadRequest(Errors.ReportEditForbidden);

            if (report.CreationTime.AddHours(24) < DateTime.Now)
                return ResponseDTO<bool>.BadRequest(Errors.EditTimeRestriction);

            // Checking tags
            List<Tag> tags = null;
            if (request.Tags?.Length > 0)
            {
                var tagsCheck = await _tagsService.CheckIsAllContainsAsync(request.Tags);
                if (!tagsCheck.Success) return ResponseDTO<bool>.Error(tagsCheck);
                tags = tagsCheck.Data;
            }

            // Checking files
            List<UploadedFile> files = null;
            if (request.FileIds?.Length > 0)
            {
                if (request.FileIds.Count() > 5) return ResponseDTO<bool>.BadRequest(Errors.TooManyFiles);

                var filesCheck = await _fileService.CheckFilesAccessAsync(request.FileIds);
                if (!filesCheck.Success) return ResponseDTO<bool>.Error(filesCheck);
                files = filesCheck.Data;
            }

            report = _mapper.Map(request, report);
            report.IsAttachmentsPrivate = request.IsFilesPrivate;
            _repo.Update(report);

            await _repo.RemoveAllTagsAsync(report.Id);
            if (tags?.Count > 0) await _repo.AddTagsAsync(report, tags);

            await _repo.RemoveAllAttachmentsAsync(report.Id);
            if (files?.Count > 0) await _repo.AddAttachmentAsync(report, files);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        // Moders and higher can change report's severity if creator set it wrong
        // A comment about new severity has created for report as moderator's name
        public async Task<ResponseDTO<bool>> SetSeverityAsync(ReportPatchEnumDTO<ReportSeverity> request)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(request.Id);
            if (!accessCheck.Success) return ResponseDTO<bool>.Error(accessCheck);
            report = accessCheck.Data;

            if (report.Severity == request.NewValue) return ResponseDTO<bool>.BadRequest();

            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            report.Severity = request.NewValue;
            report.IsSeveritySetByModerator = true;
            _repo.Update(report);

            // Sending a comment, but first check if report creator is member of product.
            // We don't send notification to report creator if report created for non-opened product
            // and report creator is not member of that product.

            // DI via class constructor leads to a crash on startup!
            var commentsService = _httpContextAccessor.HttpContext.RequestServices.GetService<ICommentsService>();
            var access = await _productService.CheckAccessAsync(report.ProductId, false, report.CreatorId);

            await commentsService.CreateWithSeverityStatusUpdateAsync(report, true, currentUser.ModeratorName, access.Success,
                request.NewValue, null, request.Comment);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        // A comment about new severity has created for report as moderator's name...
        // ...if this method called by moder or higher
        public async Task<ResponseDTO<bool>> SetStatusAsync(ReportPatchEnumDTO<ReportStatus> request)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(request.Id);
            if (!accessCheck.Success) return ResponseDTO<bool>.Error(accessCheck);
            report = accessCheck.Data;

            ReportStatus oldStatus = report.Status;
            ReportStatus newStatus = request.NewValue;

            if (oldStatus == newStatus || newStatus == ReportStatus.Open) return ResponseDTO<bool>.BadRequest();

            // Moders can change status to any, but cannot change:
            // 1. from Open to Reopen
            // 2. to NeedsCorrection or CannotReproduce without comment

            // Tester (creator) can only change:
            // 1. from NeedsCorrection or CannotReproduce to Reopened
            // 2. from ReadyForTesting to Verified or Reopened

            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);
            if (currentUser.Role != UserRole.Tester)
            {
                if (oldStatus == ReportStatus.Open && newStatus == ReportStatus.Reopened)
                    return ResponseDTO<bool>.BadRequest();

                if ((oldStatus == ReportStatus.NeedsCorrection || oldStatus == ReportStatus.CannotReproduce)
                    && string.IsNullOrEmpty(request.Comment))
                    return ResponseDTO<bool>.BadRequest(Errors.StatusRequiredComment);
            }
            else
            {
                var toReopenedFrom = new[] { ReportStatus.NeedsCorrection, ReportStatus.CannotReproduce };
                var fromReadyTo = new[] { ReportStatus.Verified, ReportStatus.Reopened };

                bool validConditions = (toReopenedFrom.Contains(oldStatus) && newStatus == ReportStatus.Reopened) ||
                    (oldStatus == ReportStatus.ReadyForTesting && fromReadyTo.Contains(newStatus));

                if (!validConditions) return ResponseDTO<bool>.BadRequest();
            }

            report.Status = newStatus;
            _repo.Update(report);

            // DI via class constructor leads to a crash on startup!
            var commentsService = _httpContextAccessor.HttpContext.RequestServices.GetService<ICommentsService>();

            bool shouldNotify = currentUser.Id != report.CreatorId;
            if (shouldNotify)
            {
                // Sending a comment, but first check if report creator is member of product.
                // We don't send notification to report creator if report created for non-opened product
                // and report creator is not member of that product.
                var access = await _productService.CheckAccessAsync(report.ProductId, false, report.CreatorId);
                shouldNotify = access.Success;
            }

            bool asModerator = currentUser.Role != UserRole.Tester;
            await commentsService.CreateWithSeverityStatusUpdateAsync(report, asModerator, currentUser.ModeratorName, shouldNotify,
                null, request.NewValue, request.Comment);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> SetReproducedAsync(int reportId, bool reproduced)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(reportId);
            if (!accessCheck.Success) return ResponseDTO<bool>.Error(accessCheck);
            report = accessCheck.Data;

            var currentUserId = _userIdProvider.UserId;
            if (report.CreatorId == currentUserId) return ResponseDTO<bool>.BadRequest();

            ReportReproduce repro = await _repo.GetReproducedUserAsync(reportId, currentUserId);
            if (reproduced)
            {
                if (repro != null) return ResponseDTO<bool>.BadRequest();
                await _repo.AddReproducedUserAsync(reportId, currentUserId);
            }
            else
            {
                if (repro == null) return ResponseDTO<bool>.BadRequest();
                _repo.DeleteReproducedUser(repro);
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<List<UserDTO>>> GetReproducedUsersAsync(int reportId)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(reportId, false);
            if (!accessCheck.Success) return ResponseDTO<List<UserDTO>>.Error(accessCheck);
            report = accessCheck.Data;

            var users = await _repo.GetReproducedUsersAsync(reportId);
            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(users);
            return new ResponseDTO<List<UserDTO>>(userDTOs);
        }

        public async Task<ResponseDTO<ReportDTO>> GetByIdAsync(int reportId)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(reportId, true);
            if (!accessCheck.Success) return ResponseDTO<ReportDTO>.Error(accessCheck);
            report = accessCheck.Data;

            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            ReportReproducesDTO reproDTO = new ReportReproducesDTO
            {
                Count = report.Reproduces.Count,
                IsReproducedByMe = report.CreatorId == currentUser.Id ? true :
                    report.Reproduces.Any(rp => rp.UserId == currentUser.Id)
            };

            ReportDTO dto = _mapper.Map<ReportDTO>(report);
            dto.Reproduces = reproDTO;
            dto.Creator = _mapper.Map<UserDTO>(report.Creator);
            dto.Product = _mapper.Map<ProductDTO>(report.Product);

            // Remove files from DTO if attachments is private.
            // Only report creator and moderators (and higher) can see private attachments
            if (report.IsAttachmentsPrivate && currentUser.Role == UserRole.Tester && report.CreatorId != currentUser.Id)
                dto.Attachments = null;

            return new ResponseDTO<ReportDTO>(dto);
        }

        public async Task<ResponseDTO<PageWithMentionsDTO<ReportDTO>>> GetAsync(GetReportsRequestDTO request)
        {
            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            // Return "bad request" if current user that is not moder (or higher)
            // trying to access other user's vulnerability reports
            if (request.CreatorId != currentUser.Id && currentUser.Role == UserRole.Tester
                && request.Severities?.Any(s => s == ReportSeverity.Vulnerability) == true)
                return ResponseDTO<PageWithMentionsDTO<ReportDTO>>.BadRequest();

            // Get found tags entities
            List<Tag> tags = null;
            if (request.Tags?.Length > 0)
            {
                var tagsCheck = await _tagsService.CheckIsAllContainsAsync(request.Tags);
                tags = tagsCheck.Data;
            }

            ReportFilter filter = new ReportFilter(request.Severities, request.ProblemTypes, request.Statuses,
                tags, request.SearchQuery, request.Reverse);

            PaginationResult<Report> result = null;
            _logger.LogInformation("Getting reports. Current user id: {0}, role: {1}", currentUser.Id, currentUser.Role.ToString());

            // If current user (CU) is tester and not moderator (and higher):
            // 1. do not return vulnerability reports NOT created by CU,
            // 2. if ProductId and CreatorId is not defined, return reports for those products that the CU is a member of,
            // 3. if ProductId is not defined and CreatorId > 0, return all reports created by CreatorId,..
            // ...excluding those created for non-open products the CU is not a member of.

            if (currentUser.Role == UserRole.Tester)
            {
                var joinedProductsResponse = await _productService.GetProductsByUserMembershipAsync(currentUser.Id,
                    ProductMemberStatus.Joined, PaginationRequestDTO.Infinity);

                if (!joinedProductsResponse.Success)
                    return ResponseDTO<PageWithMentionsDTO<ReportDTO>>.Error(joinedProductsResponse);


                var joinedProducts = joinedProductsResponse.Data;
                var joinedProductIds = joinedProducts.Items.Select(p => p.Id).ToList();
                _logger.LogInformation("Joined product ids: {0}", string.Join(", ", joinedProductIds));

                var joinedNonOpenedProductIds = joinedProducts.Items
                    .Where(p => p.AccessLevel != ProductAccessLevel.Open).Select(p => p.Id).ToList();

                _logger.LogInformation("Joined non-open product ids: {0}", string.Join(", ", joinedNonOpenedProductIds));

                // TODO: to be optimized.
                if (request.ProductId == 0 && request.CreatorId == 0)
                {
                    // Get reports that creatorId == CU && products is he joined.
                    result = await _repo.GetWithRestrictionsAsync(currentUser.Id, request.PageNumber, request.PageSize,
                        request.CreatorId, joinedNonOpenedProductIds, filter);
                }
                else if (request.ProductId == 0 && request.CreatorId > 0)
                {
                    // Get reports from creatorId's products that CU has access to 
                    result = await _repo.GetWithRestrictionsAsync(currentUser.Id, request.PageNumber, request.PageSize,
                        request.CreatorId, joinedProductIds, filter);
                }
                else
                {
                    // Get by product id. Need check access to product.
                    var productCheck = await _productService.CheckAccessAsync(request.ProductId, false);
                    if (!productCheck.Success) return ResponseDTO<PageWithMentionsDTO<ReportDTO>>.Forbidden(Errors.ForbiddenProduct);

                    result = await _repo.GetForProductWithRestrictionAsync(currentUser.Id,
                        request.PageNumber, request.PageSize, request.ProductId, request.CreatorId, filter);
                }
            }
            else
            {
                result = await _repo.GetFilteredAsync(request.PageNumber, request.PageSize,
                    request.ProductId, request.CreatorId, filter);
            }

            var mentionedUsers = result.Items.GroupBy(r => r.CreatorId).Select(r => r.FirstOrDefault().Creator).ToList();
            var mentionedProducts = result.Items.GroupBy(r => r.ProductId).Select(r => r.FirstOrDefault().Product).ToList();

            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(mentionedUsers);
            List<ProductDTO> productDTOs = _mapper.Map<List<ProductDTO>>(mentionedProducts);

            List<ReportDTO> reportDTOs = _mapper.Map<List<ReportDTO>>(result.Items);
            var data = new PageWithMentionsDTO<ReportDTO>(reportDTOs, result.TotalCount)
            {
                MentionedProducts = productDTOs,
                MentionedUsers = userDTOs
            };
            return new ResponseDTO<PageWithMentionsDTO<ReportDTO>>(data);
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(int reportId)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(reportId);
            if (!accessCheck.Success) return ResponseDTO<bool>.Error(accessCheck);
            report = accessCheck.Data;

            _repo.Delete(report);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }
    }
}
