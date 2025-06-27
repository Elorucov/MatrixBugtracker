using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.Domain.Entities.Base;

namespace MatrixBugtracker.BL.Services.Abstractions
{
    public interface IAccessService
    {
        Task<ResponseDTO<bool>> CheckAccessAsync(ICreateEntity entity);
        Task<IEnumerable<T>> GetAccessibleEntitiesAsync<T>(IEnumerable<T> entities) where T : ICreateEntity;
    }
}
