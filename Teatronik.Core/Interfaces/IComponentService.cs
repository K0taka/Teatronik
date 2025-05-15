using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IComponentService
    {
        Task<Result> AddAsync(Component component);
        Task<Result> DeleteAsync(string serialNumber);
        Task<Result<List<Component>>> GetAllAsync();
        Task<Result<List<Component>>> GetByFilterAsync(bool? isUsed = null, string? name = null, Guid[]? typeIds = null, Guid[]? kindIds = null);
        Task<Result<Component?>> GetBySerialAsync(string serialNumber);
        Task<Result> UpdateAsync(Component component);
    }
}