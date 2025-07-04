using MatrixBugtracker.BL.Resources;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class PaginationResponseDTO<T> : ResponseDTO<List<T>>
    {
        public int? TotalCount { get; private set; }

        public PaginationResponseDTO(List<T> response, int? count, int httpStatusCode = 200) : base(response, httpStatusCode)
        {
            TotalCount = count;
        }

        // TODO: try remove duplicate
        public static new PaginationResponseDTO<T> Error(int httpStatusCode, string message, Dictionary<string, string> fields = null)
        {
            return new PaginationResponseDTO<T>(default, null, httpStatusCode)
            {
                Success = false,
                ErrorMessage = message,
                ErrorFields = fields
            };
        }

        public static new PaginationResponseDTO<T> Error<T1>(ResponseDTO<T1> response)
        {
            return Error(response.HttpStatusCode, response.ErrorMessage);
        }

        public static new PaginationResponseDTO<T> BadRequest(string message = null, Dictionary<string, string> fields = null)
            => Error(400, message ?? Errors.BadRequest, fields);

        public static new PaginationResponseDTO<T> Forbidden(string message = null) => Error(403, message ?? Errors.Forbidden);
        public static new PaginationResponseDTO<T> NotFound(string message = null) => Error(404, message ?? Errors.NotFound);
    }
}
