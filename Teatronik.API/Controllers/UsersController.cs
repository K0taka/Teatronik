using Microsoft.AspNetCore.Mvc;
using Teatronik.Core.Enums;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;
using Teatronik.API.Contracts;
using Teatronik.Core.Common;

namespace Teatronik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            IRoleService roleService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
        {
            var result = await _userService.GetAllAsync();

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get users: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(u => new UserResponse(
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.RegistrationDate,
                    u.Roles
                ))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _userService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to get user {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            if (result.Value is null)
                return NotFound();

            var user = result.Value;
            return Ok(new UserResponse(
                user.Id,
                user.FullName,
                user.Email,
                user.RegistrationDate,
                user.Roles
            ));
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetByFilter(
            [FromQuery] string? name = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var result = await _userService.GetByFilter(name, fromDate, toDate);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to filter users: {Error}", result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = result.Value?
                .Select(u => new UserResponse(
                    u.Id,
                    u.FullName,
                    u.Email,
                    u.RegistrationDate,
                    u.Roles
                ))
                .ToList();

            return Ok(response ?? []);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request)
        {
            var userResult = Core.Models.User.Create(request.FullName, request.Email, request.Password);

            if (!userResult.IsSuccess)
                return BadRequest(userResult.Error);

            var addResult = await _userService.AddAsync(userResult.Value!);

            if (!addResult.IsSuccess)
            {
                _logger.LogError("Failed to create user: {Error}", addResult.Error);
                return Problem(addResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = new UserResponse(
                userResult.Value!.Id,
                userResult.Value!.FullName,
                userResult.Value!.Email,
                userResult.Value!.RegistrationDate,
                userResult.Value!.Roles
            );

            return CreatedAtAction(
                nameof(GetById),
                new { id = userResult.Value!.Id },
                response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var existingUserResult = await _userService.GetByIdAsync(id);
            if (existingUserResult.Value is null)
                return NotFound();

            var userToUpdate = existingUserResult.Value;
            var updateResult = userToUpdate.UpdateDetails(request.FullName, request.Email);

            if (!updateResult.IsSuccess)
                return BadRequest(updateResult.Error);

            var serviceResult = await _userService.UpdateAsync(userToUpdate);

            if (!serviceResult.IsSuccess)
            {
                _logger.LogError("Failed to update user {Id}: {Error}", id, serviceResult.Error);
                return Problem(serviceResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpPut("{id}/password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var userResult = await _userService.GetByIdAsync(id);
            if (userResult.Value is null)
                return NotFound();

            var passwordResult = userResult.Value.ChangePassword(request.OldPassword, request.NewPassword);

            if (!passwordResult.IsSuccess)
                return BadRequest(passwordResult.Error);

            var updateResult = await _userService.UpdateAsync(userResult.Value);

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Failed to update password for user {Id}: {Error}", id, updateResult.Error);
                return Problem(updateResult.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Id cannot be empty");

            var result = await _userService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to delete user {Id}: {Error}", id, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpPost("{userId}/roles/{roleType}")]
        public async Task<IActionResult> AddRole(Guid userId, RoleType roleType)
        {
            if (userId == Guid.Empty)
                return BadRequest("Invalid user id");

            // Получаем пользователя
            var userResult = await _userService.GetByIdAsync(userId);
            if (!userResult.IsSuccess || userResult.Value is null)
                return NotFound("User not found");

            // Получаем роль по типу через существующий сервис
            var roleResult = await GetRoleByType(roleType);
            if (!roleResult.IsSuccess)
                return NotFound(roleResult.Error);

            var result = await _userService.AddRole(userResult.Value, roleResult.Value!);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to add role {Role} to user {UserId}: {Error}",
                    roleType, userId, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [HttpDelete("{userId}/roles/{roleType}")]
        public async Task<IActionResult> RemoveRole(Guid userId, RoleType roleType)
        {
            if (userId == Guid.Empty)
                return BadRequest("Invalid user id");

            // Получаем пользователя
            var userResult = await _userService.GetByIdAsync(userId);
            if (userResult.Value is null || !userResult.IsSuccess)
                return NotFound("User not found");

            // Получаем роль по типу через существующий сервис
            var roleResult = await GetRoleByType(roleType);
            if (!roleResult.IsSuccess)
                return NotFound(roleResult.Error);

            var result = await _userService.RemoveRole(userResult.Value, roleResult.Value!);

            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to remove role {Role} from user {UserId}: {Error}",
                    roleType, userId, result.Error);
                return Problem(result.Error, statusCode: StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }


        private async Task<Result<Role?>> GetRoleByType(RoleType roleType)
        {
            // Получаем все роли и фильтруем по типу
            var allRolesResult = await _roleService.GetAllAsync();
            if (!allRolesResult.IsSuccess)
                return Result<Role?>.Fail("Failed to get roles");

            var role = allRolesResult.Value?
                .FirstOrDefault(r => r.RoleType == roleType);

            return role != null
                ? Result<Role?>.Ok(role)
                : Result<Role?>.Fail("Role not found");
        }
    }
}