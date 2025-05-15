using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IKindService
    {
        Task<Result> AddAsync(Kind kind);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<Kind>>> GetAllAsync();
        Task<Result<Kind?>> GetByIdAsync(Guid id);
        Task<Result<List<Kind>>> GetByNameAsync(string name);
        Task<Result> UpdateAsync(Kind kind);
    }
}