using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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

        public async Task AddAsync(Role role)
        {
            var entity = RoleMapper.ToEntity(role);
            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Roles
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
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

        public async Task UpdateAsync(Role role)
        {
            await _context.Roles
                .Where(r => r.Id == role.Id)
                .ExecuteUpdateAsync(e => e
                    .SetProperty(r => r.RoleName, _ => role.RoleType.ToString())
                );
        }
    }
}
