using Microsoft.AspNetCore.Mvc;
using Teatronik.API.Contracts;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Common;
using Teatronik.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly ITypeService _typeService;
        private readonly ILogger<TypesController> _logger;

        public TypesController(
            ITypeService typeService,
            ILogger<TypesController> logger)
        {
            _typeService = typeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetAll()
        {
            var result = await _typeService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get types: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(t => new TypeResponse(t.Id, t.TypeName))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _typeService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get type by id {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            return Ok(new TypeResponse(result.Value.Id, result.Value.TypeName));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> GetByName([FromQuery][Required] string name)
        {
            var result = await _typeService.GetByNameAsync(name);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to search types by name {Name}: {Error}", name, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(t => new TypeResponse(t.Id, t.TypeName))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<TypeResponse>> Create([FromBody] CreateTypeRequest request)
        {
            var typeResult = Core.Models.Type.Create(request.TypeName);
            if (!typeResult.IsSuccess)
                return BadRequest(typeResult.Error);

            var result = await _typeService.AddAsync(typeResult.Value!);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create type: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new TypeResponse(typeResult.Value!.Id, typeResult.Value!.TypeName);

            return CreatedAtAction(
                nameof(GetById),
                new { id = typeResult.Value!.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTypeRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var searchResult = await _typeService.GetByIdAsync(id);

            if (!searchResult.IsSuccess)
                return BadRequest(searchResult.Error);
            if (searchResult.Value == null)
                return NotFound();

            var tp = searchResult.Value;

            tp.UpdateName(request.TypeName);

            var updateResult = await _typeService.UpdateAsync(tp);

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update type {Id}: {Error}", id, updateResult.Error);
                return Problem(updateResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _typeService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete type {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}