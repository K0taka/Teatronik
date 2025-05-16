using Teatronik.Core.Common;
using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IPropSchemaService
    {
        Task<Result> AddAsync(PropSchema schema);
        Task<Result> DeleteAsync(Guid id);
        Task<Result<List<PropSchema>>> GetAllAsync();
        Task<Result<PropSchema?>> GetByIdAsync(Guid id);
        Task<Result<List<PropSchema>>> GetByNameAsync(string name);
        Task<Result> UpdateAsync(PropSchema schema);
    }
}