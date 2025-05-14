using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IComponentModelRepository
    {
        public Task<List<ComponentModel>> GetAllAsync();
        public Task<ComponentModel?> GetByIdAsync(Guid id);
        public Task<List<ComponentModel>> GetByFilterAsync(
            string? name = null,
            Guid[]? typeIds = null,
            Guid[]? kindIds = null
            );
        public Task AddAsync(ComponentModel componentModel);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Guid id, string modelName, Guid kindId, Guid typeId);
        public Task UpdateAsync(ComponentModel componentModel);
    }
}
