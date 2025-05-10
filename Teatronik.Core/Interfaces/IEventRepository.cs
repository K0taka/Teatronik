using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IEventRepository
    {
        public Task<List<Event>> GetAllAsync();
        public Task<Event?> GetByIdAsync(Guid id);

        public Task<List<Event>> GetByFilterAsync(
            string? name = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
            );

        public Task AddAsync(Event ev);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Event ev);
    }
}
