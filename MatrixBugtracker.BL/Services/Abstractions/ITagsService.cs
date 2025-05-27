using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ITagsService
    {
        Task<ResponseDTO<AddTagResultDTO>> AddAsync(string tagstagsComma);
    }
}
