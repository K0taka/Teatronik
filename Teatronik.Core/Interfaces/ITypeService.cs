using Teatronik.Core.Common;

namespace Teatronik.Core.Interfaces
{
    public interface ITypeService
    {
        Task<Result> AddAsync(Models.Type type);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<Models.Type>>> GetAllAsync();
        Task<Result<Models.Type?>> GetByIdAsync(Guid id);
        Task<Result<List<Models.Type>>> GetByNameAsync(string name);
        Task<Result> UpdateAsync(Models.Type type);
    }
}