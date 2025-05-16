using Microsoft.AspNetCore.Mvc;
using Teatronik.API.Contracts;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Common;
using Teatronik.Core.Models;
using Teatronik.Core.Enums;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            IRoleService roleService,
            ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponse>>> GetAll()
        {
            var result = await _roleService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get roles: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(r => new RoleResponse(r.Id, r.RoleType))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponse>> GetById(int id)
        {
            if (id < 0 || id > Enum.GetNames(typeof(RoleType)).Length)
                return BadRequest("Invalid role ID");

            var result = await _roleService.GetById(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get role by id {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            return Ok(new RoleResponse(result.Value.Id, result.Value.RoleType));
        }
    }
}