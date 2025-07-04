using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ICommentsService
    {
        Task<ResponseDTO<int?>> CreateAsync(CommentCreateDTO request);
        Task<ResponseDTO<bool>> EditAsync(CommentEditDTO request);
        Task<CommentsDTO> GetAsync(GetCommentsRequestDTO request);
        Task<ResponseDTO<bool>> DeleteAsync(int commentId);
    }
}
