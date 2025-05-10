using Teatronik.Core.Models;

namespace Teatronik.Core.Interfaces
{
    public interface IRoleRepository
    {
        public Task<List<Role>> GetAllAsync();
        public Task<Role?> GetById(int id);
        public Task AddAsync(Role role);
        public Task DeleteAsync(int id);
        public Task UpdateAsync(Role role);
    }
}
