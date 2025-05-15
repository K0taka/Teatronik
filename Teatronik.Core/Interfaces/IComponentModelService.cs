using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IComponentModelService
    {
        Task<Result> AddAsync(ComponentModel componentModel);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<ComponentModel>>> GetAllAsync();
        Task<Result<List<ComponentModel>>> GetByFilterAsync(string? name = null, Guid[]? typeIds = null, Guid[]? kindIds = null);
        Task<Result<ComponentModel?>> GetByIdAsync(Guid id);
        Task<Result> UpdateAsync(ComponentModel componentModel);
        Task<Result> UpdateAsync(Guid id, string modelName, Guid kindId, Guid typeId);
    }
}