using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly TeatronikDbContext _context;

        public TypeRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Core.Models.Type type)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Core.Models.Type>> GetAllAsync()
        {
            var entities = await _context.Types.AsNoTracking().ToListAsync();
            return entities
                .Select(TypeMapper.ToModel)
                .OfType<Core.Models.Type>()
                .ToList();
        }

        public async Task<Core.Models.Type?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Types.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return TypeMapper.ToModel(entity);
        }

        public async Task<List<Core.Models.Type>> GetByNameAsync(string name)
        {
            var entities = await _context.Types
                .AsNoTracking()
                .Where(t => EF.Functions.ILike(t.TypeName, Regex.Escape(name)))
                .ToListAsync();
            return entities
                .Select(TypeMapper.ToModel)
                .OfType<Core.Models.Type>()
                .ToList();
        }

        public Task UpdateAsync(Core.Models.Type type)
        {
            throw new NotImplementedException();
        }
    }
}
