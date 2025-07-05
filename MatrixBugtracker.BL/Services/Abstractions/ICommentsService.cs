using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ICommentsService
    {
        Task<ResponseDTO<int?>> CreateAsync(CommentCreateDTO request);
        Task CreateWithSeverityStatusUpdateAsync(Report report, bool asModerator, string moderName, bool shouldNotify,
            ReportSeverity? severity, ReportStatus? status, string text);
        Task<ResponseDTO<bool>> EditAsync(CommentEditDTO request);
        Task<ResponseDTO<PageWithMentionsDTO<CommentDTO>>> GetAsync(GetCommentsRequestDTO request);
        Task<ResponseDTO<bool>> DeleteAsync(int commentId);
    }
}
