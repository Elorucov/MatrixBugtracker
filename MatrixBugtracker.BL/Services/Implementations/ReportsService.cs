using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Enums;
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
            var access = await _productService.CheckAccessAsync(request.ProductId);
            if (!access.Success) return ResponseDTO<int?>.Error(access);

            if (request.Tags?.Length > 0)
            {
                var tagsCheck = await _tagsService.CheckIsAllContainsAsync(request.Tags);
                if (!tagsCheck.Success) return ResponseDTO<int?>.Error(tagsCheck);
            }

            return ResponseDTO<int?>.NotImplemented();
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
