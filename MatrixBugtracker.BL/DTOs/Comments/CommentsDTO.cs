using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Reports;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Comments
{
    public class CommentsDTO : PaginationResponseDTO<CommentDTO>
    {
        public CommentsDTO(List<CommentDTO> response, int? count, int httpStatusCode = 200) : base(response, count, httpStatusCode) { }

        public List<UserDTO> MentionedUsers { get; set; }


        // TODO: try remove duplicate

        public static new CommentsDTO Error(int httpStatusCode, string message, Dictionary<string, string> fields = null)
        {
            return new CommentsDTO(default, null, httpStatusCode)
            {
                Success = false,
                ErrorMessage = message,
                ErrorFields = fields
            };
        }

        public static new CommentsDTO Error<T1>(ResponseDTO<T1> response)
        {
            return Error(response.HttpStatusCode, response.ErrorMessage);
        }

        public static new CommentsDTO BadRequest(string message = null, Dictionary<string, string> fields = null)
            => Error(400, message ?? Errors.BadRequest, fields);

        public static new CommentsDTO Forbidden(string message = null) => Error(403, message ?? Errors.Forbidden);
    }
}
