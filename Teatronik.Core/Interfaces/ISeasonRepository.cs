using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface ISeasonRepository
    {
        public Task<List<Season>> GetAllAsync();
        public Task<Season?> GetByIdAsync(Guid id);

        public Task<List<Season>> GetByNameAsync(string name);

        public Task AddAsync(Season season);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Season season);
    }
}
