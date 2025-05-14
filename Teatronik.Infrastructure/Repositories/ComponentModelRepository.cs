using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class ComponentModelRepository : IComponentModelRepository
    {
        private readonly TeatronikDbContext _context;

        public ComponentModelRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ComponentModel componentModel)
        {
            var entity = ComponentModelMapper.ToEntity(componentModel);
            await _context.ComponentModels.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.ComponentModels
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<List<ComponentModel>> GetAllAsync()
        {
            var entities = await _context.ComponentModels
                .AsNoTracking()
                .ToListAsync();

            return entities
                .Select(ComponentModelMapper.ToModel)
                .OfType<ComponentModel>()
                .ToList();
        }

        public async Task<List<ComponentModel>> GetByFilterAsync(string? name = null, Guid[]? typeIds = null, Guid[]? kindIds = null)
        {
            var query = _context.ComponentModels.AsNoTracking();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(cm => EF.Functions.ILike(cm.ModelName, $"%{Regex.Escape(name)}%"));
            }
            if (typeIds?.Length > 0)
                query = query.Where(cm => typeIds.Contains(cm.TypeId));
            if (kindIds?.Length > 0)
                query = query.Where(cm => kindIds.Contains(cm.KindId));

            var entities = await query.ToListAsync();
            return entities
                .Select(e => ComponentModelMapper.ToModel(e))
                .OfType<ComponentModel>()
                .ToList();
        }

        public async Task<ComponentModel?> GetByIdAsync(Guid id)
        {
            var modelEntity = await _context.ComponentModels.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            if (modelEntity == null)
                return null;
            var model = ComponentModel.Create(modelEntity.ModelName, modelEntity.TypeId, modelEntity.KindId);
            return model.IsSuccess ? model.Value : null;
        }
        
        public async Task UpdateAsync(Guid id, string modelName, Guid kindId, Guid typeId)
        {
            await _context.ComponentModels
                .Where(cm => cm.Id == id)
                .ExecuteUpdateAsync(e => e
                    .SetProperty(cm => cm.ModelName, _ => modelName)
                    .SetProperty(cm => cm.KindId, _ => kindId)
                    .SetProperty(cm => cm.TypeId, _ => typeId));

        }

        public async Task UpdateAsync(ComponentModel componentModel)
        {
            await _context.ComponentModels
               .Where(cm => cm.Id == componentModel.Id)
               .ExecuteUpdateAsync(e => e
                   .SetProperty(cm => cm.ModelName, _ => componentModel.ModelName)
                   .SetProperty(cm => cm.KindId, _ => componentModel.KindId)
                   .SetProperty(cm => cm.TypeId, _ => componentModel.TypeId));
        }
    }
}
