using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Teatronik.Core.Enums;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TeatronikDbContext _context;

        public UserRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            var entity = UserMapper.ToEntity(user);
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Users
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            var entities = await _context.Users.AsNoTracking().ToListAsync();
            return entities
                .Select(UserMapper.ToModel)
                .OfType<User>()
                .ToList();
        }

        public async Task<List<User>> GetByFilter(string? name = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Users.AsNoTracking();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(u => EF.Functions.ILike(u.FullName, Regex.Escape(name)));
            if (fromDate != null)
                query = query.Where(u => u.RegistrationDate >=  fromDate);
            if (toDate != null)
                query = query.Where(u => u.RegistrationDate <= toDate);
            var entities = await query.ToListAsync();
            return entities
                .Select(UserMapper.ToModel)
                .OfType<User>()
                .ToList();
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            return UserMapper.ToModel(entity);
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
                throw new Exception($"User with id {user.Id} not found");

            _context.Entry(existingUser).CurrentValues.SetValues(new
            {
                user.Email,
                user.FullName,
                user.PasswordHash,
                user.RegistrationDate
            });

            var newRoles = user.Roles;
            var rolesToRemove = existingUser.Roles
                .Where(existingRole => !newRoles.Any(newRole => existingRole.RoleName == newRole.ToString()))
                .ToList();

            foreach (var role in rolesToRemove)
            {
                existingUser.Roles.Remove(role);
            }

            foreach (var roleType in newRoles)
            {
                if (!existingUser.Roles.Any(r => r.RoleName == roleType.ToString()))
                {
                    existingUser.Roles.Add(new RoleEntity
                    {
                        RoleName = roleType.ToString(),
                        Id = (int)roleType
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
