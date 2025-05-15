using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IPropService
    {
        Task<Result> AddAsybc(Prop prop);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<Prop>>> GetAllAsync();
        Task<Result<List<Prop>>> GetByFilterAsync(bool? isUsed = null, string? name = null, Guid[]? schemaIds = null, DateOnly? fromDate = null, DateOnly? toDate = null);
        Task<Result<Prop?>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(Prop prop);
    }
}