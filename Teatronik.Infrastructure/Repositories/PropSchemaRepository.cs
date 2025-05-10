using Microsoft.EntityFrameworkCore;
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

        public Task AddAsync(PropSchema propSchema)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
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

        public Task UpdateAsync(PropSchema propSchema)
        {
            throw new NotImplementedException();
        }
    }
}
