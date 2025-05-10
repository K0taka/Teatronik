using Microsoft.EntityFrameworkCore;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly TeatronikDbContext _context;

        public RoleRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Role>> GetAllAsync()
        {
            var entities = await _context.Roles.AsNoTracking().ToListAsync();
            return entities
                .Select(RoleMapper.ToModel)
                .OfType<Role>()
                .ToList();
        }

        public async Task<Role?> GetById(int id)
        {
            var entity = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            return RoleMapper.ToModel(entity);
        }

        public Task UpdateAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
