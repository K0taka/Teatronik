using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface ISeasonService
    {
        Task<Result> AddAsync(Season season);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<Season>>> GetAllAsync();
        Task<Result<Season?>> GetByIdAsynk(Guid id);
        Task<Result<List<Season>>> GetByNameAsync(string name);
        Task<Result> UpdateAsync(Season season);
    }
}