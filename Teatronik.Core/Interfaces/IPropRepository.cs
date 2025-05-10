using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IPropRepository
    {
        public Task<List<Prop>> GetAllAsync();
        public Task<Prop?> GetById(Guid id);
        public Task<List<Prop>> GetByFilterAsync(
            bool? isUsed = null,
            string? name = null,
            Guid[]? schemaIds = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null
            );

        public Task AddAsync(Prop prop);
        public Task UpdateAsync(Prop prop);
        public Task DeleteAsync(Guid id);
    }
}
