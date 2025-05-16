using Microsoft.AspNetCore.Mvc;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.API.Contracts;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentModelsController : ControllerBase
    {
        private readonly IComponentModelService _componentModelService;
        private readonly ILogger<ComponentModelsController> _logger;

        public ComponentModelsController(
            IComponentModelService componentModelService,
            ILogger<ComponentModelsController> logger)
        {
            _componentModelService = componentModelService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentModelResponse>>> GetAll()
        {
            var result = await _componentModelService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get component models: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(cm => new ComponentModelResponse(cm.Id, cm.ModelName, cm.TypeId, cm.KindId))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComponentModelResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _componentModelService.GetByIdAsync(id);

            if (!result.IsSuccess)
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);

            if (result.Value is null)
                return NotFound();

            var componentModel = result.Value;
            return Ok(new ComponentModelResponse(componentModel.Id, componentModel.ModelName, componentModel.TypeId, componentModel.KindId));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ComponentModelResponse>>> GetByFilter(
            [FromQuery] string? name = null,
            [FromQuery] Guid[]? typeIds = null,
            [FromQuery] Guid[]? kindIds = null)
        {
            var result = await _componentModelService.GetByFilterAsync(name, typeIds, kindIds);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to filter component models: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(cm => new ComponentModelResponse(cm.Id, cm.ModelName, cm.TypeId, cm.KindId))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<ComponentModelResponse>> Create([FromBody] CreateComponentModelRequest request)
        {
            var componentModelResult = ComponentModel.Create(request.ModelName, request.TypeId, request.KindId);

            if (!componentModelResult.IsSuccess)
                return BadRequest($"{componentModelResult.Error}");


            var result = await _componentModelService.AddAsync(componentModelResult.Value!);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create component model: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new ComponentModelResponse(
                componentModelResult.Value!.Id,
                componentModelResult.Value!.ModelName,
                componentModelResult.Value!.TypeId,
                componentModelResult.Value!.KindId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = componentModelResult.Value!.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateComponentModelRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var searchResult = await _componentModelService.GetByIdAsync(id);

            if (!searchResult.IsSuccess)
                return BadRequest(searchResult.Error);

            if (searchResult.Value == null)
                return NotFound();

            var componentModel = searchResult.Value;

            componentModel.UpdateName(request.ModelName);
            componentModel.UpdateKind(request.KindId);
            componentModel.UpdateType(request.TypeId);

            var result = await _componentModelService.UpdateAsync(componentModel);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update component model {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _componentModelService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete component model {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}