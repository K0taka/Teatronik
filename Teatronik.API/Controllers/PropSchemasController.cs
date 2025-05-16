using Microsoft.AspNetCore.Mvc;
using Teatronik.API.Contracts;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropSchemasController : ControllerBase
    {
        private readonly IPropSchemaService _propSchemaService;
        private readonly ILogger<PropSchemasController> _logger;

        public PropSchemasController(
            IPropSchemaService propSchemaService,
            ILogger<PropSchemasController> logger)
        {
            _propSchemaService = propSchemaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropSchemaResponse>>> GetAll()
        {
            var result = await _propSchemaService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get prop schemas: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(s => new PropSchemaResponse(
                    s.Id,
                    s.SchemaName,
                    s.Length,
                    s.Width,
                    s.Height))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropSchemaResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _propSchemaService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get prop schema by id {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            var schema = result.Value;
            return Ok(new PropSchemaResponse(
                schema.Id,
                schema.SchemaName,
                schema.Length,
                schema.Width,
                schema.Height));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PropSchemaResponse>>> GetByName([FromQuery][Required] string name)
        {
            var result = await _propSchemaService.GetByNameAsync(name);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to search prop schemas by name {Name}: {Error}", name, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(s => new PropSchemaResponse(
                    s.Id,
                    s.SchemaName,
                    s.Length,
                    s.Width,
                    s.Height))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<PropSchemaResponse>> Create([FromBody] CreatePropSchemaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var schemaRes = PropSchema.Create(request.SchemaName, request.Length, request.Width, request.Height);

            if(!schemaRes.IsSuccess)
                return BadRequest(schemaRes.Error);


            var schema = schemaRes.Value!;
            var result = await _propSchemaService.AddAsync(schema);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create prop schema: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new PropSchemaResponse(
                schema.Id,
                schema.SchemaName,
                schema.Length,
                schema.Width,
                schema.Height);

            return CreatedAtAction(
                nameof(GetById),
                new { id = schema.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropSchemaRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var searchRes = await _propSchemaService.GetByIdAsync(id);

            if (!searchRes.IsSuccess)
                return BadRequest(searchRes.Error);

            if (searchRes.Value == null)
                return NotFound();

            var schema = searchRes.Value;

            schema.UpdateName(request.SchemaName);
            schema.UpdateLength(request.Length);
            schema.UpdateWidth(request.Width);
            schema.UpdateHeight(request.Height);
            var updateResult = await _propSchemaService.UpdateAsync(schema);

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update prop schema {Id}: {Error}", id, updateResult.Error);
                return Problem(updateResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _propSchemaService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete prop schema {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}