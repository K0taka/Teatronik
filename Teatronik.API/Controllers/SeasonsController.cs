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
    public class SeasonsController : ControllerBase
    {
        private readonly ISeasonService _seasonService;
        private readonly ILogger<SeasonsController> _logger;

        public SeasonsController(
            ISeasonService seasonService,
            ILogger<SeasonsController> logger)
        {
            _seasonService = seasonService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeasonResponse>>> GetAll()
        {
            var result = await _seasonService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get seasons: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(s => new SeasonResponse(s.Id, s.SeasonName))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeasonResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _seasonService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get season by id {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            return Ok(new SeasonResponse(result.Value.Id, result.Value.SeasonName));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<SeasonResponse>>> GetByName([FromQuery][Required] string name)
        {
            var result = await _seasonService.GetByNameAsync(name);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to search seasons by name {Name}: {Error}", name, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(s => new SeasonResponse(s.Id, s.SeasonName))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<SeasonResponse>> Create([FromBody] CreateSeasonRequest request)
        {
            var seasonResult = Season.Create(request.Name);
            if (!seasonResult.IsSuccess)
                return BadRequest(seasonResult.Error);

            var season = seasonResult.Value!;
            var result = await _seasonService.AddAsync(season);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to create season: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new SeasonResponse(season.Id, season.SeasonName);

            return CreatedAtAction(
                nameof(GetById),
                new { id = season.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSeasonRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var searchResult = await _seasonService.GetByIdAsync(id);

            if (!searchResult.IsSuccess)
                return BadRequest(searchResult.Error);

            if (searchResult.Value == null)
                return NotFound();

            var season = searchResult.Value;

            season.UpdateName(request.Name);

            var updateResult = await _seasonService.UpdateAsync(season);

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update season {Id}: {Error}", id, updateResult.Error);
                return Problem(updateResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _seasonService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete season {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}