using Microsoft.AspNetCore.Mvc;
using Teatronik.API.Contracts;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Common;
using Teatronik.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Teatronik.Application;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropsController : ControllerBase
    {
        private readonly IPropService _propService;
        private readonly ILogger<PropsController> _logger;

        public PropsController(
            IPropService propService,
            ILogger<PropsController> logger)
        {
            _propService = propService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropResponse>>> GetAll()
        {
            var result = await _propService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get props: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(p => new PropResponse(
                    p.Id,
                    p.PropName,
                    p.Created,
                    p.SchemaId))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _propService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get prop by id {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            var prop = result.Value;
            return Ok(new PropResponse(
                prop.Id,
                prop.PropName,
                prop.Created,
                prop.SchemaId));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<PropResponse>>> GetByFilter(
            [FromQuery] bool? isUsed = null,
            [FromQuery] string? name = null,
            [FromQuery] Guid[]? schemaIds = null,
            [FromQuery] DateOnly? fromDate = null,
            [FromQuery] DateOnly? toDate = null)
        {
            var result = await _propService.GetByFilterAsync(
                isUsed,
                name,
                schemaIds,
                fromDate,
                toDate);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to filter props: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(p => new PropResponse(
                    p.Id,
                    p.PropName,
                    p.Created,
                    p.SchemaId))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<PropResponse>> Create([FromBody] CreatePropRequest request)
        {
            var propRes = Prop.Create(request.PropName, request.Created, request.SchemaId);

            if (!propRes.IsSuccess)
                return BadRequest(propRes.Error);

            var prop = propRes.Value!;

            var result = await _propService.AddAsync(prop);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create prop: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new PropResponse(
                prop.Id,
                prop.PropName,
                prop.Created,
                prop.SchemaId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = prop.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var searchRes = await _propService.GetByIdAsync(id);

            if (!searchRes.IsSuccess)
                return BadRequest(searchRes.Error);
            if (searchRes.Value == null)
                return NotFound();

            var prop = searchRes.Value;

            prop.UpdateName(request.PropName);
            var updateResult = await _propService.UpdateAsync(prop);

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update prop {Id}: {Error}", id, updateResult.Error);
                return Problem(updateResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _propService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete prop {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}