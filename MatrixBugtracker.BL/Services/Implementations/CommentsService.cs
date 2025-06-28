using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;

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

            List<UploadedFile> files = null;
            if (request.FileIds?.Length > 0)
            {
                var filesCheck = await _fileService.CheckFilesAccessAsync(request.FileIds);
                if (!filesCheck.Success) return ResponseDTO<int?>.Error(filesCheck);
                files = filesCheck.Response;
            }

            Comment comment = _mapper.Map<Comment>(request);
            comment.IsAttachmentsPrivate = request.IsFilesPrivate; // TODO: mapper.
            await _repo.AddAsync(comment);

            if (files?.Count > 0) await _repo.AddAttachmentAsync(comment, files);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<int?>(comment.Id);
        }

        public async Task<ResponseDTO<bool>> EditAsync(CommentEditDTO request)
        {
            Comment comment = await _repo.GetByIdAsync(request.Id);
            if (comment == null) return ResponseDTO<bool>.NotFound();

            // Admins can edit all comments (but why?), others can edit only own created comment
            var access = await _accessService.CheckAccessAsync(comment);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            // TODO: if this comment is not last, editing is not allowed

            List<UploadedFile> files = null;
            if (request.FileIds?.Length > 0)
            {
                var filesCheck = await _fileService.CheckFilesAccessAsync(request.FileIds);
                if (!filesCheck.Success) return ResponseDTO<bool>.Error(filesCheck);
                files = filesCheck.Response;
            }

            comment = _mapper.Map(request, comment);
            _repo.Update(comment);

            await _repo.RemoveAllAttachmentsAsync(comment.Id);
            if (files?.Count > 0) await _repo.AddAttachmentAsync(comment, files);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }
    }
}
