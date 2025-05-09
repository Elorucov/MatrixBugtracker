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

        public ResponseDTO(T response, int httpStatusCode = StatusCodes.Status200OK)
        {
            Response = response;
            Success = true;
            HttpStatusCode = httpStatusCode;
        }

        public static ResponseDTO<T> Error(int httpStatusCode, string message)
        {
            return new ResponseDTO<T>(default, httpStatusCode)
            {
                Success = false,
                ErrorMessage = message
            };
        }
    }
}
