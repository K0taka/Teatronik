using Microsoft.AspNetCore.Mvc;
using Teatronik.API.Contracts;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KindsController : ControllerBase
    {
        private readonly IKindService _kindService;
        private readonly ILogger<KindsController> _logger;

        public KindsController(
            IKindService kindService,
            ILogger<KindsController> logger)
        {
            _kindService = kindService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KindResponse>>> GetAll()
        {
            var result = await _kindService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get kinds: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(k => new KindResponse(k.Id, k.KindName))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KindResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _kindService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get kind by id {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            return Ok(new KindResponse(result.Value.Id, result.Value.KindName));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<KindResponse>>> GetByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name cannot be empty");

            var result = await _kindService.GetByNameAsync(name);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to search kinds by name {Name}: {Error}", name, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(k => new KindResponse(k.Id, k.KindName))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<KindResponse>> Create([FromBody] CreateKindRequest request)
        {
            var kindResult = Kind.Create(request.KindName);

            if (!kindResult.IsSuccess)
                return BadRequest(kindResult.Error);

            var result = await _kindService.AddAsync(kindResult.Value!);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create kind: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new KindResponse(kindResult.Value!.Id, kindResult.Value!.KindName);

            return CreatedAtAction(
                nameof(GetById),
                new { id = kindResult.Value!.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateKindRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var searchResult = await _kindService.GetByIdAsync(id);

            if (!searchResult.IsSuccess)
                return BadRequest(searchResult.Error);

            if (searchResult.Value == null)
                return NotFound();

            var kind = searchResult.Value;

            kind.UpdateName(request.KindName);
            var updateResult = await _kindService.UpdateAsync(kind);

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update kind {Id}: {Error}", id, updateResult.Error);
                return Problem(updateResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _kindService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete kind {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}