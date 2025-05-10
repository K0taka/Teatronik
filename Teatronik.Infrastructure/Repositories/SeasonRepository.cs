using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly TeatronikDbContext _context;

        public SeasonRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Season season)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Season>> GetAllAsync()
        {
            var entities = await _context.Seasons
                .AsNoTracking()
                .ToListAsync();
            return entities
                .Select(SeasonMapper.ToModel)
                .OfType<Season>()
                .ToList();
        }

        public async Task<Season?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return SeasonMapper.ToModel(entity);
        }

        public async Task<List<Season>> GetByNameAsync(string name)
        {
            var entities = await _context.Seasons
                .AsNoTracking()
                .Where(s => EF.Functions.ILike(s.SeasonName, Regex.Escape(name)))
                .ToListAsync();
            return entities
                .Select(SeasonMapper.ToModel)
                .OfType<Season>()
                .ToList();
        }

        public Task UpdateAsync(Season season)
        {
            throw new NotImplementedException();
        }
    }
}
