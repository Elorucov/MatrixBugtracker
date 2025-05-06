using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class ResponseDTO<T>
    {
        public T Response { get; private set; }
        public bool Success { get; private set; }
        public int HttpStatusCode { get; private set; }
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
