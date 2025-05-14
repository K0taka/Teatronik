using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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

        public async Task AddAsync(Season season)
        {
            var entity = SeasonMapper.ToEntity(season);
            await _context.Seasons.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Seasons
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
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

        public async Task UpdateAsync(Season season)
        {
            await _context.Seasons
                .Where(s => s.Id == season.Id)
                .ExecuteUpdateAsync(e => e
                    .SetProperty(s => s.SeasonName, _ => season.SeasonName)
                );
        }
    }
}
