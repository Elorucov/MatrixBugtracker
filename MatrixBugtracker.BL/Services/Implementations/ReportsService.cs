using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Models;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class ReportsService : IReportsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessService _accessService;
        private readonly IFileService _fileService;
        private readonly IProductService _productService;
        private readonly ITagsService _tagsService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly IReportRepository _repo;

        public ReportsService(IUnitOfWork unitOfWork, IAccessService accessService,
            IFileService fileService, IProductService productService, ITagsService tagsService,
            IUserService userService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _fileService = fileService;
            _productService = productService;
            _tagsService = tagsService;
            _userService = userService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<IReportRepository>();
        }

        public async Task<ResponseDTO<int?>> CreateAsync(ReportCreateDTO request)
        {
            Product product = null;
            var access = await _productService.CheckAccessAsync(request.ProductId);
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
            await _repo.AddAsync(report);

            if (tags?.Count > 0) await _repo.AddTagsAsync(report, tags);
            if (files?.Count > 0) await _repo.AddAttachmentAsync(report, files);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<int?>(report.Id);
        }

        public async Task<ResponseDTO<bool>> EditAsync(ReportEditDTO request)
        {
            Report report = await _repo.GetByIdWithIncludesAsync(request.Id);
            if (report == null) return ResponseDTO<bool>.NotFound();

            var accessCheck = await _accessService.CheckAccessAsync(report);
            if (!accessCheck.Success) return accessCheck;

            // TODO: ThenInclude ProductMembers when getting a report to optimize
            Product product = report.Product;
            var access = await _productService.CheckAccessAsync(product.Id);
            if (!access.Success) return ResponseDTO<bool>.Error(access);
            product = access.Response;

            // We don't allow to edit reports whose status already changed or if severity changed by moderator
            // TODO: only tester (creator) can not edit, moders and higher can.
            if (report.Status != 0 || report.IsSeveritySetByModerator) return ResponseDTO<bool>.BadRequest(Errors.ReportEditForbidden);

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
            report.UpdateTime = DateTime.Now;
            _repo.Update(report);

            await _repo.RemoveAllTagsAsync(report.Id);
            if (tags?.Count > 0) await _repo.AddTagsAsync(report, tags);

            await _repo.RemoveAllAttachmentsAsync(report.Id);
            if (files?.Count > 0) await _repo.AddAttachmentAsync(report, files);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        // Moders and higher can change report's severity if creator set it wrong
        public async Task<ResponseDTO<bool>> SetSeverityAsync(ReportPatchEnumDTO<ReportSeverity> request)
        {
            Report report = await _repo.GetByIdWithIncludesAsync(request.Id);
            if (report == null) return ResponseDTO<bool>.NotFound();

            var accessCheck = await _accessService.CheckAccessAsync(report);
            if (!accessCheck.Success) return accessCheck;

            // TODO: ThenInclude ProductMembers when getting a report to optimize
            Product product = report.Product;
            var access = await _productService.CheckAccessAsync(product.Id);
            if (!access.Success) return ResponseDTO<bool>.Error(access);
            product = access.Response;

            report.Severity = request.NewValue;
            report.UpdateTime = DateTime.Now;
            report.IsSeveritySetByModerator = true;
            _repo.Update(report);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> SetStatusAsync(ReportPatchEnumDTO<ReportStatus> request)
        {
            Report report = await _repo.GetByIdWithIncludesAsync(request.Id);
            if (report == null) return ResponseDTO<bool>.NotFound();

            var accessCheck = await _accessService.CheckAccessAsync(report);
            if (!accessCheck.Success) return accessCheck;

            // TODO: ThenInclude ProductMembers when getting a report to optimize
            Product product = report.Product;
            var access = await _productService.CheckAccessAsync(product.Id);
            if (!access.Success) return ResponseDTO<bool>.Error(access);
            product = access.Response;

            ReportStatus oldStatus = report.Status;
            ReportStatus newStatus = request.NewValue;

            if (oldStatus == newStatus) return ResponseDTO<bool>.BadRequest();

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
            } else
            {
                var toReopenedFrom = new[] { ReportStatus.NeedsCorrection, ReportStatus.CannotReproduce };
                var fromReadyTo = new[] { ReportStatus.Verified, ReportStatus.Reopened };

                bool validConditions = (toReopenedFrom.Contains(oldStatus) && newStatus == ReportStatus.Reopened) ||
                    (oldStatus == ReportStatus.ReadyForTesting && fromReadyTo.Contains(newStatus));

                if (!validConditions) return ResponseDTO<bool>.BadRequest();
            }

            report.Status = newStatus;
            report.UpdateTime = DateTime.Now;
            _repo.Update(report);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<ReportDTO>> GetByIdAsync(int reportId)
        {
            Report report = await _repo.GetByIdWithIncludesAsync(reportId);
            if (report == null) return ResponseDTO<ReportDTO>.NotFound();

            // To be optimized, if we include ProductMembers in _repo.GetByIdWithIncludesAsync
            var access = await _productService.CheckAccessAsync(report.Product.Id);
            if (!access.Success) return ResponseDTO<ReportDTO>.Error(access);

            // Check if authorized user can access to vulnerability report
            // If user is tester, he can view only vulnerabilities that created by own
            User currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);
            if (report.Severity == ReportSeverity.Vulnerability && currentUser.Role == UserRole.Tester)
            {
                if (report.CreatorId != currentUser.Id) return ResponseDTO<ReportDTO>.Forbidden();
            }

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
                return (PaginationResponseDTO<ReportDTO>)PaginationResponseDTO<ReportDTO>.BadRequest();

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
            // 2. if ProductId and CreatorId is not defined, return reports for those products that the CU is a member of 
            // 3. if ProductId is not defined and CreatorId > 0, return all reports created by CreatorId,..
            // ...excluding those created for non-open products the CU is not a member of.
            if (currentUser.Role == UserRole.Tester)
            {
                var joinedProductsResponse = await _productService.GetProductsByUserMembershipAsync(currentUser.Id, ProductMemberStatus.Joined, PaginationRequestDTO.Infinity);
                if (!joinedProductsResponse.Success) return (PaginationResponseDTO<ReportDTO>)PaginationResponseDTO<ReportDTO>.Error(joinedProductsResponse);

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
                    var productCheck = await _productService.CheckAccessAsync(request.ProductId);
                    if (!productCheck.Success) return (PaginationResponseDTO<ReportDTO>)PaginationResponseDTO<ReportDTO>.Forbidden(Errors.ForbiddenProduct);

                    result = await _repo.GetForProductWithRestrictionAsync(currentUser.Id, request.Number, request.Size, request.ProductId, request.CreatorId, filter);
                }
            }
            else
            {
                result = await _repo.GetFilteredAsync(request.Number, request.Size, request.ProductId, request.CreatorId);
            }

            List<ReportDTO> reportDTOs = _mapper.Map<List<ReportDTO>>(result.Items);
            return new PaginationResponseDTO<ReportDTO>(reportDTOs, result.TotalCount);
        }

        public ResponseDTO<ReportEnumsDTO> GetEnumValues()
        {
            var problemTypes = EnumExtensions.GetTranslatedEnums<ReportProblemType>();
            var severities = EnumExtensions.GetTranslatedEnums<ReportSeverity>();
            var statuses = EnumExtensions.GetTranslatedEnums<ReportStatus>();

            ReportEnumsDTO response = new ReportEnumsDTO
            {
                ProblemTypes = problemTypes,
                Severities = severities,
                Statuses = statuses
            };

            return new ResponseDTO<ReportEnumsDTO>(response);
        }
    }
}
