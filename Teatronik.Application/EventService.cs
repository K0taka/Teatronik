using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<Result<List<Event>>> GetAllAsync() =>
            Result<List<Event>>.Ok(await _eventRepository.GetAllAsync());

        public async Task<Result<Event?>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result<Event?>.Fail("id was empty");
            return Result<Event?>.Ok(await _eventRepository.GetByIdAsync(id));
        }

        public async Task<Result<List<Event>>> GetByFilterAsync(
            string? name = null,
            DateTime? fromDate = null,
            DateTime? toDate = null
            ) => Result<List<Event>>.Ok(await _eventRepository.GetByFilterAsync(name, fromDate, toDate));

        public async Task<Result> AddAsync(Event ev)
        {
            if (ev == null)
                return Result.Fail("event was null");
            await _eventRepository.AddAsync(ev);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Fail($"{nameof(id)} cannot be deleted");
            await _eventRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Event ev)
        {
            if (ev == null)
                return Result.Fail("Event was null");
            await _eventRepository.UpdateAsync(ev);
            return Result.Ok();
        }

        public async Task<Result> AddProp(Event ev, Prop prop)
        {
            if (prop == null)
                return Result.Fail("Prop was null");
            if (ev == null)
                return Result.Fail("Event was null");

            var add = ev.AddProp(prop);
            if (!add.IsSuccess)
                return Result.Fail("Already added");
            
            await _eventRepository.UpdateAsync(ev);
            return Result.Ok();
        }

        public async Task<Result> RemoveProp(Event ev, Prop prop)
        {
            if (prop == null)
                return Result.Fail("Prop was null");
            if (ev == null)
                return Result.Fail("Event was null");

            var rev = ev.RemoveProp(prop);
            if (!rev.IsSuccess)
                return Result.Fail("Wasn't assigned");
            await _eventRepository.UpdateAsync(ev);
            return Result.Ok();
        }
    }
}
