using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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

        public async Task AddAsync(Kind kind)
        {
            var entity = KindMapper.ToEntity(kind);
            await _context.Kinds.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Kinds
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
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

        public async Task UpdateAsync(Kind kind)
        {
            await _context.Kinds
                .Where(k => k.Id == kind.Id)
                .ExecuteUpdateAsync(e => e
                    .SetProperty(k => k.KindName, _ => kind.KindName)
                );
        }
    }
}
