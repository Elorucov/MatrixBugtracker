using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class CommentsService : ICommentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccessService _accessService;
        private readonly IFileService _fileService;
        private readonly IReportsService _reportsService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly ICommentRepository _repo;

        public CommentsService(IUnitOfWork unitOfWork, IAccessService accessService,
            IFileService fileService, IReportsService reportsService, INotificationService notificationService,
        IUserService userService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _fileService = fileService;
            _reportsService = reportsService;
            _notificationService = notificationService;
            _userService = userService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<ICommentRepository>();
        }

        public async Task<ResponseDTO<int?>> CreateAsync(CommentCreateDTO request)
        {
            var accessCheck = await _reportsService.CheckAccessAsync(request.ReportId);
            if (!accessCheck.Success) return ResponseDTO<int?>.Error(accessCheck);

            int reportCreatorId = accessCheck.Response.CreatorId;

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

            await _notificationService.SendToUserAsync(reportCreatorId, true, UserNotificationKind.ReportCommentAdded, LinkedEntityType.Comment, comment.Id);

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
            if (comment.CreationTime.AddHours(24) < DateTime.Now) return ResponseDTO<bool>.BadRequest(Errors.EditTimeRestriction);

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

        public async Task<PaginationResponseDTO<CommentDTO>> GetAsync(GetCommentsRequestDTO request)
        {
            var accessCheck = await _reportsService.CheckAccessAsync(request.ReportId);
            if (!accessCheck.Success) return PaginationResponseDTO<CommentDTO>.Error(accessCheck);

            User currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);
            var result = await _repo.GetForReportAsync(request.ReportId, request.Number, request.Size);

            List<CommentDTO> commentDTOs = new List<CommentDTO>();

            foreach (var comment in result.Items)
            {
                CommentDTO dto = _mapper.Map<CommentDTO>(comment);
                var author = comment.Creator;
                if (currentUser.Role != Domain.Enums.UserRole.Tester)
                {
                    dto.Author = new CommentAuthorDTO
                    {
                        UserId = comment.CreatorId,
                        Name = !comment.AsModerator ? $"{author.FirstName} {author.LastName}" : $"{author.ModeratorName} ({author.FirstName} {author.LastName})"
                    };
                }
                dto.Author = new CommentAuthorDTO
                {
                    UserId = !comment.AsModerator ? comment.CreatorId : null,
                    Name = !comment.AsModerator ? $"{author.FirstName} {author.LastName}" : author.ModeratorName
                };
                commentDTOs.Add(dto);
            }

            return new PaginationResponseDTO<CommentDTO>(commentDTOs, result.TotalCount);
        }

        public async Task<ResponseDTO<bool>> DeleteAsync(int commentId)
        {
            Comment comment = await _repo.GetByIdAsync(commentId);
            if (comment == null) return ResponseDTO<bool>.NotFound();

            // Admins can delete comments, others can delete only own created comment
            var access = await _accessService.CheckAccessAsync(comment);
            if (!access.Success) return ResponseDTO<bool>.Error(access);

            // TODO: if this comment is not last, deleting is not allowed
            if (comment.CreationTime.AddHours(24) < DateTime.Now) return ResponseDTO<bool>.BadRequest(Errors.DeletionTimeRestriction);

            if (comment.NewSeverity.HasValue || comment.NewStatus.HasValue)
                return ResponseDTO<bool>.BadRequest(Errors.ForbiddenSeverityStatusCommentDeletion);

            _repo.Delete(comment);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }
    }
}
