using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class PropRepository : IPropRepository
    {
        private readonly TeatronikDbContext _context;

        public PropRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Prop prop)
        {
            var entity = PropMapper.ToEntity(prop);
            await _context.Props.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Props
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<Prop>> GetAllAsync()
        {
            var entities = await _context.Props.AsNoTracking().ToListAsync();
            return entities
                .Select(PropMapper.ToModel)
                .OfType<Prop>()
                .ToList();
        }

        public async Task<List<Prop>> GetByFilterAsync(
            bool? isUsed = null,
            string? name = null, 
            Guid[]? schemaIds = null, 
            DateOnly? fromDate = null, 
            DateOnly? toDate = null
            )
        {
            var query = _context.Props.AsNoTracking();
            if (isUsed.HasValue)
                query = isUsed.Value
                    ? query.Where(p => p.Events.Count != 0)
                    : query.Where(p => p.Events.Count == 0);

            if(!string.IsNullOrEmpty(name))
                query = query.Where(p => EF.Functions.ILike(p.PropName, Regex.Escape(name)));

            if(schemaIds?.Length > 0)
                query = query.Where(p => schemaIds.Contains(p.SchemaId));

            if (fromDate != null)
                query = query.Where(p => p.Created >= fromDate.Value);
            if(toDate != null)
                query = query.Where(p => p.Created <= toDate.Value);

            var entities = await query.ToListAsync();
            return entities
                .Select(PropMapper.ToModel)
                .OfType<Prop>()
                .ToList();
        }

        public async Task<Prop?> GetById(Guid id)
        {
            var entity = await _context.Props.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return PropMapper.ToModel(entity);
        }

        public async Task UpdateAsync(Prop prop)
        {
            await _context.Props
                .Where(p => p.Id == prop.Id)
                .ExecuteUpdateAsync(e => e
                    .SetProperty(p => p.Created, _ => prop.Created)
                    .SetProperty(p => p.PropName, _ => prop.PropName)
                    .SetProperty(p => p.SchemaId, _ => prop.SchemaId)
                );
        }
    }
}
