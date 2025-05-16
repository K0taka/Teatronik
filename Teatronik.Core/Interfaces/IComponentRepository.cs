using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IComponentRepository
    {
        public Task<List<Component>> GetAllAsync();
        public Task<Component?> GetBySerialAsync(string serialNumber);
        
        public Task<List<Component>> GetByFilterAsync(
            bool? isUsed = null,
            string? name = null,
            Guid[]? typeIds = null,
            Guid[]? kindIds = null,
            Guid? modelId = null,
            Guid? propId = null
            );

        public Task AddAsync(Component component);
        public Task DeleteAsync(string serialNumber);
        public Task UpdateAsync(Component component);
    }
}
