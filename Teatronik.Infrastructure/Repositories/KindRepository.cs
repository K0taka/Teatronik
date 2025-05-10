using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class KindRepository : IKindRepository
    {
        private readonly TeatronikDbContext _context;

        public KindRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Kind kind)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Kind>> GetAllAsync()
        {
            var entities = await _context.Kinds.AsNoTracking().ToListAsync();
            return entities
                .Select(KindMapper.ToModel)
                .OfType<Kind>()
                .ToList();
        }

        public async Task<Kind?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Kinds.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return KindMapper.ToModel(entity);
        }

        public async Task<List<Kind>> GetByNameAsync(string name)
        {
            var entities = await _context.Kinds
                .AsNoTracking()
                .Where(k => EF.Functions.ILike(k.KindName, Regex.Escape(name)))
                .ToListAsync();
            return entities
                .Select(KindMapper.ToModel)
                .OfType<Kind>()
                .ToList();
        }

        public Task UpdateAsync(Kind kind)
        {
            throw new NotImplementedException();
        }
    }
}
