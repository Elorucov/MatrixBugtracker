using MatrixBugtracker.BL.DTOs.Comments;
using MatrixBugtracker.BL.DTOs.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ICommentsService
    {
        Task<ResponseDTO<int?>> CreateAsync(CommentCreateDTO request);
    }
}
