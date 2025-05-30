using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Tags;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface ITagsService
    {
        Task<ResponseDTO<AddTagResultDTO>> AddAsync(string tagsComma);
        Task<ResponseDTO<List<TagDTO>>> GetAsync(bool withArchived);
        Task<ResponseDTO<bool>> CheckIsAllContainsAsync(string[] tags);
        Task<ResponseDTO<bool>> SetArchiveFlag(string tagName, bool isArchived);
    }
}
