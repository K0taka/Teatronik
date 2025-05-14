using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class PropSchemaRepository : IPropSchemaRepository
    {
        private readonly TeatronikDbContext _context;

        public PropSchemaRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PropSchema propSchema)
        {
            var entity = PropSchemaMapper.ToEntity(propSchema);
            await _context.PropSchemas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.PropSchemas
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<PropSchema>> GetAllAsync()
        {
            var entities = await _context.PropSchemas.AsNoTracking().ToListAsync();
            return entities
                .Select(PropSchemaMapper.ToModel)
                .OfType<PropSchema>()
                .ToList();
        }

        public async Task<PropSchema?> GetByIdAsync(Guid id)
        {
            var entity = await _context.PropSchemas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return PropSchemaMapper.ToModel(entity);
        }

        public async Task<List<PropSchema>> GetByName(string name)
        {
            var entities = await _context.PropSchemas
                .AsNoTracking()
                .Where(ps => EF.Functions.ILike(ps.SchemaName, Regex.Escape(name)))
                .ToListAsync();
            return entities
                .Select(PropSchemaMapper.ToModel)
                .OfType<PropSchema>()
                .ToList();
        }

        public async Task UpdateAsync(PropSchema propSchema)
        {
            await _context.PropSchemas
                .Where(s => s.Id == propSchema.Id)
                .ExecuteUpdateAsync(e => e
                    .SetProperty(s => s.SchemaName, _ => propSchema.SchemaName)
                    .SetProperty(s => s.Length, _ => propSchema.Length)
                    .SetProperty(s => s.Width, _ => propSchema.Width)
                    .SetProperty(s => s.Height, _ => propSchema.Height)
                );
        }
    }
}
