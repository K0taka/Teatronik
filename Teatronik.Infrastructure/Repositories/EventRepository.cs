using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Mappers;

namespace Teatronik.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly TeatronikDbContext _context;

        public EventRepository(TeatronikDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Event ev)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Event>> GetAllAsync()
        {
            var entities = await _context.Events.AsNoTracking().ToListAsync();
            return entities
                .Select(EventMapper.ToModel)
                .OfType<Event>()
                .ToList();
        }

        public async Task<List<Event>> GetByFilterAsync(string? name = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Events.AsNoTracking();
            if(!string.IsNullOrEmpty(name))
                query = query.Where(e => EF.Functions.ILike(e.EventName, Regex.Escape(name)));
            if(fromDate != null)
                query = query.Where(e => e.DateTime >= fromDate);
            if (toDate != null)
                query = query.Where(e => e.DateTime <= toDate);
            var entities = await query.ToListAsync();
            return entities
                .Select(EventMapper.ToModel)
                .OfType<Event>()
                .ToList();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            return EventMapper.ToModel(entity);
        }

        public async Task UpdateAsync(Event ev)
        {
            throw new NotImplementedException();
        }
    }
}
