using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IPropSchemaRepository
    {
        public Task<List<PropSchema>> GetAllAsync();

        public Task<PropSchema?> GetByIdAsync(Guid id);

        public Task<List<PropSchema>> GetByName(string name);

        public Task AddAsync(PropSchema propSchema);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(PropSchema propSchema);
    }
}
