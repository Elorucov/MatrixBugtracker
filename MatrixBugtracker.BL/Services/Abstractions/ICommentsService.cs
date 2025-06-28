using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ICommentsService
    {
        Task<ResponseDTO<int?>> CreateAsync(CommentCreateDTO request);
    }
}
