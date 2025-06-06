using AutoMapper;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Tags;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class TagsService : ITagsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly ITagRepository _repo;

        public TagsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            _repo = _unitOfWork.GetRepository<ITagRepository>();
        }

        public async Task<ResponseDTO<AddTagResultDTO>> AddAsync(string[] tags)
        {
            if (tags.Length == 0) return ResponseDTO<AddTagResultDTO>.BadRequest();

            tags.ToLower();

            for (int i = 0; i < tags.Length; i++)
            {
                string tag = tags[i];
                if (string.IsNullOrWhiteSpace(tag))
                    return ResponseDTO<AddTagResultDTO>.BadRequest(string.Format(Errors.TagIsEmpty, i));

                if (tag.Length > 64)
                    return ResponseDTO<AddTagResultDTO>.BadRequest(string.Format(Errors.TagIsLong, i));
            }

            var existTags = (await _repo.GetIntersectingAsync(tags)).Select(t => t.Name);
            if (existTags.Count() == tags.Length) return ResponseDTO<AddTagResultDTO>.BadRequest(Errors.AllTagsAlreadyExist);

            var newTags = tags.Where(t => !existTags.Contains(t)).ToArray();
            await _repo.AddBatchAsync(newTags);
            await _unitOfWork.CommitAsync();

            AddTagResultDTO result = new AddTagResultDTO
            {
                AddedCount = newTags.Count(),
                AlreadyExist = existTags.ToList()
            };

            return new ResponseDTO<AddTagResultDTO>(result);
        }

        public async Task<ResponseDTO<List<TagDTO>>> GetAsync(bool withArchived)
        {
            List<Tag> tags = withArchived ? await _repo.GetAllAsync() : await _repo.GetUnarchivedAsync();
            List<TagDTO> tagDTOs = _mapper.Map<List<TagDTO>>(tags);
            return new ResponseDTO<List<TagDTO>>(tagDTOs);
        }

        // Checking if ALL tags contains in DB
        // Returns tags that not found in DB to display in error response.
        public async Task<ResponseDTO<bool>> CheckIsAllContainsAsync(string[] tags)
        {
            List<Tag> existTags = await _repo.GetIntersectingAsync(tags);
            if (existTags.Count < tags.Length) return ResponseDTO<bool>.BadRequest(Errors.NotFoundSomeTags);
            if (existTags.Any(t => t.IsArchived)) return ResponseDTO<bool>.BadRequest(Errors.NotFoundSomeTags);

            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> SetArchiveFlag(string tagName, bool isArchived)
        {
            Tag tag = await _repo.GetByNameAsync(tagName);
            if (tag == null) return ResponseDTO<bool>.NotFound();

            tag.IsArchived = isArchived;

            _repo.Update(tag);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }
    }
}
