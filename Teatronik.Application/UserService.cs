using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> AddAsync(User user)
        {
            if (user == null)
                return Result.Fail("user was null");
            await _userRepository.AddAsync(user);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Fail("id was empty");
            await _userRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result<List<User>>> GetAllAsync() =>
            Result<List<User>>.Ok(await _userRepository.GetAllAsync());

        public async Task<Result<List<User>>> GetByFilter(
            string? name = null,
            DateTime? fromDate = null,
            DateTime? toDate = null) =>
            Result<List<User>>.Ok(await _userRepository.GetByFilter(name, fromDate, toDate));

        public async Task<Result<User?>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result<User?>.Fail("id is empty");

            return Result<User?>.Ok(await _userRepository.GetByIdAsync(id));
        }

        public async Task<Result> UpdateAsync(User user)
        {
            if (user == null)
                return Result.Fail("User was null");
            await _userRepository.UpdateAsync(user);
            return Result.Ok();
        }

        public async Task<Result> AddRole(User user, Role role)
        {
            var add = user.AssignRole(role.RoleType);
            if (!add.IsSuccess)
                return Result.Fail("Role is already assigned");
            await _userRepository.UpdateAsync(user);
            return Result.Ok();
        }
        public async Task<Result> RemoveRole(User user, Role role)
        {
            var remove = user.RemoveRole(role.RoleType);
            if (!remove.IsSuccess)
                return Result.Fail("Role is not assigned");
            await _userRepository.UpdateAsync(user);
            return Result.Ok();
        }
    }
}
