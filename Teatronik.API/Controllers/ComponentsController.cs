using Microsoft.AspNetCore.Mvc;
using Teatronik.API.Contracts;
using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentsController> _logger;

        public ComponentsController(
            IComponentService componentService,
            ILogger<ComponentsController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentResponse>>> GetAll()
        {
            var result = await _componentService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get components: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(c => MapToResponse(c))
                .ToList();

            return Ok(response ?? new List<ComponentResponse>());
        }

        [HttpGet("serial/{serialNumber}")]
        public async Task<ActionResult<ComponentResponse>> GetBySerial(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return BadRequest("Serial number cannot be empty");

            var result = await _componentService.GetBySerialAsync(serialNumber);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get component by serial: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            return Ok(MapToResponse(result.Value));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<ComponentResponse>>> GetByFilter(
            [FromQuery] Guid? modelId = null,
            [FromQuery] Guid? propId = null,
            [FromQuery] bool? isUsed = null,
            [FromQuery] string? name = null,
            [FromQuery] Guid[]? typeIds = null,
            [FromQuery] Guid[]? kindIds = null)
        {
    
            bool? actualIsUsed = propId != null ? true : isUsed;

            var result = await _componentService.GetByFilterAsync(
                isUsed: actualIsUsed,
                name: name,
                typeIds: typeIds,
                kindIds: kindIds,
                modelId: modelId,
                propId: propId);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to filter components: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(MapToResponse)
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<ComponentResponse>> Create([FromBody] CreateComponentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creationResult = Component.Create(
                request.SerialNumber,
                request.AcquisitionDate,
                request.ModelId,
                request.PropId);

            if (!creationResult.IsSuccess)
                return BadRequest(creationResult.Error);

            var component = creationResult.Value!;
            var addResult = await _componentService.AddAsync(component);

            if (!addResult.IsSuccess)
            {
                _logger.LogError("Failed to create component: {Error}", addResult.Error);
                return Problem(addResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = MapToResponse(component);

            return CreatedAtAction(
                nameof(GetBySerial),
                new { serialNumber = component.SerialNumber },
                response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateComponentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Получаем существующий компонент
            var getResult = await _componentService.GetBySerialAsync(request.SerialNumber);
            if (!getResult.IsSuccess || getResult.Value is null)
                return NotFound();

            var existingComponent = getResult.Value;

            // Создаем обновленный компонент
            var updateResult = Component.Create(
                request.SerialNumber,
                request.AcquisitionDate,
                request.ModelId,
                request.PropId);

            if (!updateResult.IsSuccess)
                return BadRequest(updateResult.Error);

            var updatedComponent = updateResult.Value!;

            // Обновляем данные
            var result = await _componentService.UpdateAsync(updatedComponent);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to update component {Serial}: {Error}",
                    request.SerialNumber, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{serialNumber}")]
        public async Task<IActionResult> Delete(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return BadRequest("Serial number cannot be empty");

            if (serialNumber.Length > Component.MAX_SERIALNUMBER_LENGTH)
                return BadRequest($"Serial number must be not greater than {Component.MAX_SERIALNUMBER_LENGTH}");

            var result = await _componentService.DeleteAsync(serialNumber);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete component {Serial}: {Error}",
                    serialNumber, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        private static ComponentResponse MapToResponse(Component component)
        {
            return new ComponentResponse(
                component.SerialNumber,
                component.AcquisitionDate,
                component.ModelId,
                component.PropId);
        }
    }
}