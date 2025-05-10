using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IKindRepository
    {
        public Task<List<Kind>> GetAllAsync();
        public Task<Kind?> GetByIdAsync(Guid id);
        public Task<List<Kind>> GetByNameAsync(string name);

        public Task AddAsync(Kind kind);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(Kind kind);
    }
}
