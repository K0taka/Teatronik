using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(Guid id);
        public Task<List<User>> GetByFilter(
            string? name = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
            );
        public Task AddAsync(User user);
        public Task DeleteAsync(Guid id);
        public Task UpdateAsync(User user);
    }
}
