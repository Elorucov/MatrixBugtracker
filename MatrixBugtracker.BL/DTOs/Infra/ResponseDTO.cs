using MatrixBugtracker.BL.Resources;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class ResponseDTO<T>
    {
        [JsonIgnore]
        public int HttpStatusCode { get; private set; }
        public bool Success { get; private set; }
        public T Response { get; private set; }
        public string ErrorMessage { get; private set; }
        public Dictionary<string, string> ErrorFields { get; private set; }

        public ResponseDTO(T response, int httpStatusCode = StatusCodes.Status200OK)
        {
            Response = response;
            Success = true;
            HttpStatusCode = httpStatusCode;
        }

        public static ResponseDTO<T> Error(int httpStatusCode, string message, Dictionary<string, string> fields = null)
        {
            return new ResponseDTO<T>(default, httpStatusCode)
            {
                Success = false,
                ErrorMessage = message,
                ErrorFields = fields
            };
        }

        public static ResponseDTO<T> BadRequest(string message = null, Dictionary<string, string> fields = null)
            => ResponseDTO<T>.Error(400, message ?? Errors.BadRequest, fields);

        public static ResponseDTO<T> Unauthorized(string message = null) => ResponseDTO<T>.Error(401, message ?? Errors.Unauthorized);
        public static ResponseDTO<T> Forbidden(string message = null) => ResponseDTO<T>.Error(403, message ?? Errors.Forbidden);
        public static ResponseDTO<T> NotFound() => ResponseDTO<T>.Error(404, Errors.NotFound);
        public static ResponseDTO<T> NotImplemented() => ResponseDTO<T>.Error(500, Errors.NotImplemented);
    }
}
