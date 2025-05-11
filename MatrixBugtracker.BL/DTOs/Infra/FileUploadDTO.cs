using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.DTOs.Infra
{
    public class FileUploadDTO
    {
        public IFormFile File { get; set; }
        public bool IsPhoto { get; set; }
    }
}
