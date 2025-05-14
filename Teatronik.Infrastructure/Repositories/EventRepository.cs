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
            var entity = EventMapper.ToEntity(ev);
            await _context.Events.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Events
                .Where(cm => cm.Id == id)
                .ExecuteDeleteAsync();
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
            var existingEvent = await _context.Events
                .Include(e => e.Props)
                .FirstOrDefaultAsync(e => e.Id == ev.Id);

            if (existingEvent == null)
                throw new Exception($"Event with id {ev.Id} not found");

            
            _context.Entry(existingEvent).CurrentValues.SetValues(new
            {
                ev.EventName,
                ev.Theme,
                ev.DateTime,
                ev.Duration,
                ev.SeasonId,
                ev.Spectators
            });

          
            var propsToRemove = existingEvent.Props
                .Where(existingProp => !ev.Props.Any(newProp => newProp.Id == existingProp.Id))
                .ToList();

            foreach (var prop in propsToRemove)
            {
                existingEvent.Props.Remove(prop);
            }

            
            foreach (var newProp in ev.Props)
            {
                var existingProp = existingEvent.Props
                    .FirstOrDefault(p => p.Id == newProp.Id);

                if (existingProp == null)
                {
                    
                    existingEvent.Props.Add(PropMapper.ToEntity(newProp));
                }
                else
                {
                    
                    _context.Entry(existingProp).CurrentValues.SetValues(new
                    {
                        newProp.PropName,
                        newProp.SchemaId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
