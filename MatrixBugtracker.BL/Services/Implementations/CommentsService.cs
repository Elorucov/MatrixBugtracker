using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class CommentsService : ICommentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessService _accessService;
        private readonly IFileService _fileService;
        private readonly IReportsService _reportsService;
        private readonly ITagsService _tagsService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly ICommentRepository _repo;

        public CommentsService(IUnitOfWork unitOfWork, IAccessService accessService,
            IFileService fileService, IReportsService reportsService, ITagsService tagsService,
            IUserService userService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _fileService = fileService;
            _reportsService = reportsService;
            _tagsService = tagsService;
            _userService = userService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<ICommentRepository>();
        }

        public async Task<ResponseDTO<int?>> CreateAsync(CommentCreateDTO request)
        {
            var accessCheck = await _reportsService.CheckAccessAsync(request.ReportId);
            if (!accessCheck.Success) return ResponseDTO<int?>.Error(accessCheck);

            return ResponseDTO<int?>.NotImplemented();
        }
    }
}
