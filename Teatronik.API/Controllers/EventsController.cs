using Microsoft.AspNetCore.Mvc;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.API.Contracts;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IPropService _propService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(
            IEventService eventService,
            IPropService propService,
            ILogger<EventsController> logger)
        {
            _eventService = eventService;
            _propService = propService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponse>>> GetAll()
        {
            var result = await _eventService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get events: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(e => new EventResponse(
                    e.Id,
                    e.EventName,
                    e.DateTime,
                    e.Duration,
                    e.SeasonId,
                    e.Theme,
                    e.Spectators,
                    e.Props.Select(p => new PropResponse(p.Id, p.PropName, p.Created, p.SchemaId)).ToList()
                ))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _eventService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get event {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            var eventEntity = result.Value;
            return Ok(new EventResponse(
                eventEntity.Id,
                eventEntity.EventName,
                eventEntity.DateTime,
                eventEntity.Duration,
                eventEntity.SeasonId,
                eventEntity.Theme,
                eventEntity.Spectators,
                eventEntity.Props.Select(p => new PropResponse(p.Id, p.PropName, p.Created, p.SchemaId)).ToList()
            ));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<EventResponse>>> GetByFilter(
            [FromQuery] string? name = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await _eventService.GetByFilterAsync(name, fromDate, toDate);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to filter events: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(e => new EventResponse(
                    e.Id,
                    e.EventName,
                    e.DateTime,
                    e.Duration,
                    e.SeasonId,
                    e.Theme,
                    e.Spectators,
                    e.Props.Select(p => new PropResponse(p.Id, p.PropName, p.Created, p.SchemaId)).ToList()
                ))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<EventResponse>> Create([FromBody] CreateEventRequest request)
        {
            var eventResult= Event.Create(request.EventName, request.DateTime, request.Duration, request.SeasonId, request.Theme, request.Spectators);

            if(!eventResult.IsSuccess)
                return BadRequest(eventResult.Error);

            var @event = eventResult.Value!;
            var result = await _eventService.AddAsync(@event);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create event: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new EventResponse(
                @event.Id,
                @event.EventName,
                @event.DateTime,
                @event.Duration,
                @event.SeasonId,
                @event.Theme,
                @event.Spectators,
                @event.Props.Select(p => new PropResponse(p.Id, p.PropName, p.Created, p.SchemaId)).ToList()
            );

            return CreatedAtAction(
                nameof(GetById),
                new { id = @event.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEventResult = await _eventService.GetByIdAsync(id);
            if (!existingEventResult.IsSuccess)
                return Problem(existingEventResult.Error, statusCode: StatusCodes.Status500InternalServerError);

            if (existingEventResult.Value is null)
                return NotFound();

            var eventToUpdate = existingEventResult.Value;
            eventToUpdate.UpdateName(request.EventName);
            eventToUpdate.UpdateDateTime(request.DateTime);
            eventToUpdate.UpdateDuration(request.Duration);
            eventToUpdate.UpdateSeasonId(request.SeasonId);
            eventToUpdate.UpdateTheme(request.Theme);
            eventToUpdate.UpdateSpectators(request.Spectators);

            var result = await _eventService.UpdateAsync(eventToUpdate);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update event {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _eventService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete event {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpPost("{eventId}/props/{propId}")]
        public async Task<IActionResult> AddProp(Guid eventId, Guid propId)
        {
            if (eventId == Guid.Empty || propId == Guid.Empty)
                return BadRequest("Invalid ids");

            var eventResult = await _eventService.GetByIdAsync(eventId);
            if (eventResult.Value is null)
                return NotFound("Event not found");

            var propResult = await _propService.GetByIdAsync(propId);
            if (propResult.Value is null)
                return NotFound("Prop not found");

            var result = await _eventService.AddProp(eventResult.Value, propResult.Value);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to add prop {PropId} to event {EventId}: {Error}",
                    propId, eventId, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{eventId}/props/{propId}")]
        public async Task<IActionResult> RemoveProp(Guid eventId, Guid propId)
        {
            if (eventId == Guid.Empty || propId == Guid.Empty)
                return BadRequest("Invalid ids");

            var eventResult = await _eventService.GetByIdAsync(eventId);
            if (eventResult.Value is null)
                return NotFound("Event not found");

            var propResult = await _propService.GetByIdAsync(propId);
            if (propResult.Value is null)
                return NotFound("Prop not found");

            var result = await _eventService.RemoveProp(eventResult.Value, propResult.Value);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to remove prop {PropId} from event {EventId}: {Error}",
                    propId, eventId, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}