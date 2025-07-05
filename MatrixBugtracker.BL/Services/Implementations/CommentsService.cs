using AutoMapper;
using Azure.Core;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Extensions;
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
        private readonly IProductsService _productService;
        private readonly IReportsService _reportsService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        private readonly ICommentRepository _repo;

        public CommentsService(IUnitOfWork unitOfWork, IAccessService accessService, IProductsService productService,
            IFileService fileService, IReportsService reportsService, INotificationService notificationService,
            IUserService userService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _accessService = accessService;
            _fileService = fileService;
            _productService = productService;
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
            var report = accessCheck.Data;

            int reportCreatorId = accessCheck.Data.CreatorId;
            var currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);

            if (request.AsModerator && currentUser.Role == UserRole.Tester) return ResponseDTO<int?>.BadRequest();

            List<UploadedFile> files = null;
            if (request.FileIds?.Length > 0)
            {
                var filesCheck = await _fileService.CheckFilesAccessAsync(request.FileIds);
                if (!filesCheck.Success) return ResponseDTO<int?>.Error(filesCheck);
                files = filesCheck.Data;
            }

            Comment comment = _mapper.Map<Comment>(request);
            comment.IsAttachmentsPrivate = request.IsFilesPrivate;
            await _repo.AddAsync(comment);

            if (files?.Count > 0) await _repo.AddAttachmentAsync(comment, files);

            if (currentUser.Id != reportCreatorId)
            {
                // Check if report creator is member of product.
                // We don't send notification if report created for non-opened product and report creator is not member of that product.
                var access = await _productService.CheckAccessAsync(report.ProductId, false, report.CreatorId);
                if (access.Success)
                {
                    string userName = request.AsModerator ? currentUser.ModeratorName : $"{currentUser.FirstName} {currentUser.LastName}";
                    var notificationText = string.Format(Common.CommentAddedNotification, userName, report.Title.Truncate(64), request.Text.Truncate(128));
                    await _notificationService.SendToUserAsync(reportCreatorId, true, UserNotificationKind.ReportCommentAdded, notificationText, LinkedEntityType.Report, request.ReportId);
                }
            }

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<int?>(comment.Id);
        }

        public async Task CreateWithSeverityStatusUpdateAsync(Report report, bool asModerator, string moderName, bool shouldNotify,
            ReportSeverity? severity, ReportStatus? status, string text)
        {
            if (severity.HasValue && status.HasValue) throw new ArgumentException("Only one of these parameters are allowed: severity OR status");
            if (!severity.HasValue && !status.HasValue) throw new ArgumentException("severity or status required");

            Comment comment = new Comment
            {
                ReportId = report.Id,
                NewSeverity = severity,
                NewStatus = status,
                Text = text,
                AsModerator = asModerator
            };

            await _repo.AddAsync(comment);

            if (shouldNotify)
            {
                string parameterWithoutComment = severity.HasValue ? Common.ReportSeverityChanged : Common.ReportStatusChanged;
                string parameterWithComment = severity.HasValue ? Common.ReportSeverityChangedWithComment : Common.ReportStatusChangedWithComment;

                string notificationResourceKey = string.IsNullOrEmpty(text)
                        ? parameterWithoutComment : parameterWithComment;

                string toValueString = severity.HasValue
                    ? EnumValues.ResourceManager.GetString($"{nameof(ReportSeverity)}_{severity}")
                    : EnumValues.ResourceManager.GetString($"{nameof(ReportStatus)}_{status}");

                var notificationText = string.Format(notificationResourceKey, moderName,
                    report.Title.Truncate(64), toValueString, text.Truncate(128));

                await _notificationService.SendToUserAsync(report.CreatorId, true,
                    UserNotificationKind.ReportCommentAdded, notificationText, LinkedEntityType.Report, report.Id);
            }
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
                files = filesCheck.Data;
            }

            comment = _mapper.Map(request, comment);
            comment.IsAttachmentsPrivate = request.IsFilesPrivate;
            _repo.Update(comment);

            await _repo.RemoveAllAttachmentsAsync(comment.Id);
            if (files?.Count > 0) await _repo.AddAttachmentAsync(comment, files);

            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<PageWithMentionsDTO<CommentDTO>>> GetAsync(GetCommentsRequestDTO request)
        {
            var accessCheck = await _reportsService.CheckAccessAsync(request.ReportId);
            if (!accessCheck.Success) return ResponseDTO<PageWithMentionsDTO<CommentDTO>>.Error(accessCheck);

            User currentUser = await _userService.GetSingleUserAsync(_userIdProvider.UserId);
            var result = await _repo.GetForReportAsync(request.ReportId, request.PageNumber, request.PageSize);

            List<CommentDTO> commentDTOs = new List<CommentDTO>();
            List<User> authors = new List<User>();

            foreach (var comment in result.Items)
            {
                CommentDTO dto = _mapper.Map<CommentDTO>(comment);
                var author = comment.Creator;
                if (currentUser.Role != UserRole.Tester)
                {
                    dto.Author = new CommentAuthorDTO
                    {
                        UserId = comment.CreatorId,
                        Name = !comment.AsModerator
                        ? $"{author.FirstName} {author.LastName}"
                        : $"{author.ModeratorName} ({author.FirstName} {author.LastName})"
                    };
                    authors.Add(author);
                }
                else
                {
                    dto.Author = new CommentAuthorDTO
                    {
                        UserId = !comment.AsModerator ? comment.CreatorId : null,
                        Name = !comment.AsModerator ? $"{author.FirstName} {author.LastName}" : author.ModeratorName
                    };
                    if (!comment.AsModerator) authors.Add(author);
                }
                commentDTOs.Add(dto);
            }

            // Remove duplicates
            var mentionedUsers = authors.GroupBy(r => r.Id).Select(r => r.FirstOrDefault()).ToList();

            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(mentionedUsers);

            var data = new PageWithMentionsDTO<CommentDTO>(commentDTOs, result.TotalCount)
            {
                MentionedUsers = userDTOs
            };

            return new ResponseDTO<PageWithMentionsDTO<CommentDTO>>(data);
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
