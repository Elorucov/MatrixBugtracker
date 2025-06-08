using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Tags;
using MatrixBugtracker.DAL.Entities;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ITagsService
    {
        Task<ResponseDTO<AddTagResultDTO>> AddAsync(string[] tags);
        Task<ResponseDTO<List<TagDTO>>> GetAsync(bool withArchived);
        Task<ResponseDTO<List<Tag>>> CheckIsAllContainsAsync(string[] tags);
        Task<ResponseDTO<bool>> SetArchiveFlag(string tagName, bool isArchived);
    }
}
