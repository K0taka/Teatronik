using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly TeatronikDbContext _context;

        public ComponentRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Component component)
        {

        }

        public async Task DeleteAsync(string serialNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Component>> GetAllAsync()
        {
            var entities = await _context.Components.AsNoTracking().ToListAsync();
            return entities
                .Select(e => ComponentMapper.ToModel(e))
                .OfType<Component>()
                .ToList();

        }

        public async Task<List<Component>> GetByFilterAsync(
            bool? isUsed = null,
            string? name = null,
            Guid[]? typeIds = null,
            Guid[]? kindIds = null)
        {
            var query = _context.Components.AsNoTracking();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => EF.Functions.ILike(c.Model.ModelName, $"%{Regex.Escape(name)}%"));
            }

            if (typeIds?.Length > 0)
                query = query.Where(c => typeIds.Contains(c.Model.TypeId));

            if (kindIds?.Length > 0)
                query = query.Where(c => kindIds.Contains(c.Model.KindId));

            if (isUsed.HasValue)
                query = isUsed.Value
                    ? query.Where(c => c.Prop != null)
                    : query.Where(c => c.Prop == null);

            var entities = await query.ToListAsync();
            return entities
                .Select(ComponentMapper.ToModel)
                .OfType<Component>()
                .ToList();
        }

        public async Task<Component?> GetBySerialAsync(string serialNumber)
        {
            var entity = await _context.Components
                .AsNoTracking()
                .FirstOrDefaultAsync(c => EF.Functions.ILike(c.SerialNumber, $"%{Regex.Escape(serialNumber)}%"));
            return ComponentMapper.ToModel(entity);
        }

        public async Task UpdateAsync(Component component)
        {
            throw new NotImplementedException();
        }
    }
}
