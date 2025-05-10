using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
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

        public Task AddAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllAsync()
        {
            var entities = await _context.Users.AsNoTracking().ToListAsync();
            return entities
                .Select(UserMapper.ToModel)
                .OfType<User>()
                .ToList();
        }

        public async Task<List<User>> GetByFilter(string? name, DateTime? fromDate = null, DateTime? toDate = null)
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

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
