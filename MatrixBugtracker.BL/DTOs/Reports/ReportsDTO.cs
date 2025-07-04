using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Products;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.DTOs.Reports
{
    public class ReportsDTO : PaginationResponseDTO<ReportDTO>
    {
        public ReportsDTO(List<ReportDTO> response, int? count, int httpStatusCode = 200) : base(response, count, httpStatusCode) { }

        public List<UserDTO> MentionedUsers { get; set; }
        public List<ProductDTO> MentionedProducts { get; set; }


        // TODO: try remove duplicate or find a way to call base class's method!

        public static new ReportsDTO Error(int httpStatusCode, string message, Dictionary<string, string> fields = null)
        {
            return new ReportsDTO(default, null, httpStatusCode)
            {
                Success = false,
                ErrorMessage = message,
                ErrorFields = fields
            };
        }

        public static new ReportsDTO Error<T1>(ResponseDTO<T1> response)
        {
            return Error(response.HttpStatusCode, response.ErrorMessage);
        }

        public static new ReportsDTO BadRequest(string message = null, Dictionary<string, string> fields = null)
            => Error(400, message ?? Errors.BadRequest, fields);

        public static new ReportsDTO Forbidden(string message = null) => Error(403, message ?? Errors.Forbidden);
    }
}
