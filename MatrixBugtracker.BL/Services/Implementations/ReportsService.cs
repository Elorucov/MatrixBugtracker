using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;
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
    public class ReportsService : IReportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IProductService _productService;
        private readonly ITagsService _tagsService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly IReportRepository _repo;
        private readonly ICommentRepository _commentRepo;

        public ReportsService(IUnitOfWork unitOfWork, IFileService fileService,
            IProductService productService, ITagsService tagsService, INotificationService notificationService,
            IUserService userService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _productService = productService;
            _tagsService = tagsService;
            _notificationService = notificationService;
            _userService = userService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<IReportRepository>();
            _commentRepo = _unitOfWork.GetRepository<ICommentRepository>();
        }

        public async Task<ResponseDTO<Report>> CheckAccessAsync(int reportId)
        {
            Report report = await _repo.GetByIdAsync(reportId);
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
            product = access.Response;

            if (product.IsOver) return ResponseDTO<int?>.BadRequest(Errors.ProductTestingIsOver);

            List<Tag> tags = null;
            if (request.Tags?.Length > 0)
            {
                var tagsCheck = await _tagsService.CheckIsAllContainsAsync(request.Tags);
                if (!tagsCheck.Success) return ResponseDTO<int?>.Error(tagsCheck);
                tags = tagsCheck.Response;
            }

            List<UploadedFile> files = null;
            if (request.FileIds?.Length > 0)
            {
                var filesCheck = await _fileService.CheckFilesAccessAsync(request.FileIds);
                if (!filesCheck.Success) return ResponseDTO<int?>.Error(filesCheck);
                files = filesCheck.Response;
            }

            Report report = _mapper.Map<Report>(request);
            report.IsAttachmentsPrivate = request.IsFilesPrivate; // TODO: mapper.
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
            report = accessCheck.Response;

            // We don't allow to edit reports whose status already changed or if severity changed by moderator
            // TODO: only tester (creator) can not edit, moders and higher can.
            if (report.Status != 0 || report.IsSeveritySetByModerator) return ResponseDTO<bool>.BadRequest(Errors.ReportEditForbidden);

            if (report.CreationTime.AddHours(24) < DateTime.Now) return ResponseDTO<bool>.BadRequest(Errors.EditTimeRestriction);

            List<Tag> tags = null;
            if (request.Tags?.Length > 0)
            {
                var tagsCheck = await _tagsService.CheckIsAllContainsAsync(request.Tags);
                if (!tagsCheck.Success) return ResponseDTO<bool>.Error(tagsCheck);
                tags = tagsCheck.Response;
            }

            List<UploadedFile> files = null;
            if (request.FileIds?.Length > 0)
            {
                var filesCheck = await _fileService.CheckFilesAccessAsync(request.FileIds);
                if (!filesCheck.Success) return ResponseDTO<bool>.Error(filesCheck);
                files = filesCheck.Response;
            }

            report = _mapper.Map(request, report);
            report.IsAttachmentsPrivate = request.IsFilesPrivate; // TODO: mapper.
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
            report = accessCheck.Response;

            if (report.Severity == request.NewValue) return ResponseDTO<bool>.BadRequest();

            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            report.Severity = request.NewValue;
            report.IsSeveritySetByModerator = true;
            _repo.Update(report);

            Comment comment = new Comment
            {
                ReportId = report.Id,
                NewSeverity = request.NewValue,
                Text = request.Comment,
                AsModerator = true
            };

            await _commentRepo.AddAsync(comment);

            string notificationResourceKey = string.IsNullOrEmpty(request.Comment) ? Common.ReportSeverityChanged : Common.ReportSeverityChangedWithComment;
            string severityStr = EnumValues.ResourceManager.GetString($"{nameof(ReportSeverity)}_{request.NewValue}");
            var notificationText = string.Format(notificationResourceKey, currentUser.ModeratorName, report.Title.Truncate(64), severityStr, request.Comment.Truncate(128));
            await _notificationService.SendToUserAsync(report.CreatorId, true, UserNotificationKind.ReportCommentAdded, notificationText, LinkedEntityType.Report, report.Id);

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
            report = accessCheck.Response;

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

                if ((oldStatus == ReportStatus.NeedsCorrection || oldStatus == ReportStatus.CannotReproduce) && string.IsNullOrEmpty(request.Comment))
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

            Comment comment = new Comment
            {
                ReportId = report.Id,
                NewStatus = newStatus,
                Text = request.Comment,
                AsModerator = currentUser.Role != UserRole.Tester
            };

            await _commentRepo.AddAsync(comment);

            if (currentUser.Id != report.CreatorId)
            {
                string notificationResourceKey = string.IsNullOrEmpty(request.Comment) ? Common.ReportStatusChanged : Common.ReportStatusChangedWithComment;
                string statusStr = EnumValues.ResourceManager.GetString($"{nameof(ReportStatus)}_{request.NewValue}");
                var notificationText = string.Format(notificationResourceKey, currentUser.ModeratorName, report.Title.Truncate(64), statusStr, request.Comment.Truncate(128));
                await _notificationService.SendToUserAsync(report.CreatorId, true, UserNotificationKind.ReportCommentAdded, notificationText, LinkedEntityType.Report, report.Id);
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> SetReproducedAsync(int reportId, bool reproduced)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(reportId);
            if (!accessCheck.Success) return ResponseDTO<bool>.Error(accessCheck);
            report = accessCheck.Response;

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

        public async Task<ResponseDTO<ReportDTO>> GetByIdAsync(int reportId)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(reportId);
            if (!accessCheck.Success) return ResponseDTO<ReportDTO>.Error(accessCheck);
            report = accessCheck.Response;

            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            ReportReproducesDTO reproDTO = new ReportReproducesDTO
            {
                Count = report.Reproduces.Count,
                IsReproducedByMe = report.CreatorId == currentUser.Id ? true :
                    report.Reproduces.Any(rp => rp.UserId == currentUser.Id)
            };

            ReportDTO dto = _mapper.Map<ReportDTO>(report);
            dto.Reproduces = reproDTO;

            // Remove files from DTO if attachments is private.
            // Only report creator and moderators (and higher) can see private attachments
            if (report.IsAttachmentsPrivate && currentUser.Role == UserRole.Tester && report.CreatorId != currentUser.Id)
                dto.Attachments = null;

            return new ResponseDTO<ReportDTO>(dto);
        }

        public async Task<PaginationResponseDTO<ReportDTO>> GetAsync(GetReportsRequestDTO request)
        {
            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            // Return "bad request" if current user that is not moder (or higher)
            // trying to access other user's vulnerability reports
            if (request.CreatorId != currentUser.Id && currentUser.Role == UserRole.Tester
                && request.Severities?.Count == 1 && request.Severities[0] == ReportSeverity.Vulnerability)
                return PaginationResponseDTO<ReportDTO>.BadRequest();

            // Get found tags entities
            List<Tag> tags = null;
            if (request.Tags?.Length > 0)
            {
                var tagsCheck = await _tagsService.CheckIsAllContainsAsync(request.Tags);
                tags = tagsCheck.Response;
            }

            ReportFilter filter = new ReportFilter(request.Severities, request.ProblemTypes, request.Statuses, tags);

            PaginationResult<Report> result = null;

            // If current user (CU) is tester and not moderator (and higher):
            // 1. do not return vulnerability reports NOT created by CU,
            // 2. if ProductId and CreatorId is not defined, return reports for those products that the CU is a member of,
            // 3. if ProductId is not defined and CreatorId > 0, return all reports created by CreatorId,..
            // ...excluding those created for non-open products the CU is not a member of.
            if (currentUser.Role == UserRole.Tester)
            {
                var joinedProductsResponse = await _productService.GetProductsByUserMembershipAsync(currentUser.Id, ProductMemberStatus.Joined, PaginationRequestDTO.Infinity);
                if (!joinedProductsResponse.Success) return PaginationResponseDTO<ReportDTO>.Error(joinedProductsResponse);

                var joinedProducts = joinedProductsResponse.Response;
                var joinedProductIds = joinedProducts.Select(p => p.Id).ToList();
                var joinedNonOpenedProductIds = joinedProducts.Where(p => p.AccessLevel != ProductAccessLevel.Open).Select(p => p.Id).ToList();

                // TODO: to be optimized.
                if (request.ProductId == 0 && request.CreatorId == 0)
                {
                    // Get reports that creatorId == CU && products is he joined.
                    result = await _repo.GetWithRestrictionsAsync(currentUser.Id, request.Number, request.Size, request.CreatorId, joinedNonOpenedProductIds, filter);

                }
                else if (request.ProductId == 0 && request.CreatorId > 0)
                {
                    // Get reports from creatorId's products that CU has access to 
                    result = await _repo.GetWithRestrictionsAsync(currentUser.Id, request.Number, request.Size, request.CreatorId, joinedProductIds, filter);
                }
                else
                {
                    // Get by product id. Need check access to product.
                    var productCheck = await _productService.CheckAccessAsync(request.ProductId, false);
                    if (!productCheck.Success) return PaginationResponseDTO<ReportDTO>.Forbidden(Errors.ForbiddenProduct);

                    result = await _repo.GetForProductWithRestrictionAsync(currentUser.Id, request.Number, request.Size, request.ProductId, request.CreatorId, filter);
                }
            }
            else
            {
                result = await _repo.GetFilteredAsync(request.Number, request.Size, request.ProductId, request.CreatorId, filter);
            }

            List<ReportDTO> reportDTOs = _mapper.Map<List<ReportDTO>>(result.Items);
            return new PaginationResponseDTO<ReportDTO>(reportDTOs, result.TotalCount);
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(int reportId)
        {
            Report report = null;
            var accessCheck = await CheckAccessAsync(reportId);
            if (!accessCheck.Success) return ResponseDTO<bool>.Error(accessCheck);
            report = accessCheck.Response;

            _repo.Delete(report);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }
    }
}
