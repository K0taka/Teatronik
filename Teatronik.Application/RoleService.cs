using Teatronik.Core.Common;
using Teatronik.Core.Enums;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result<List<Role>>> GetAllAsync() =>
            Result<List<Role>>.Ok(await _roleRepository.GetAllAsync());

        public async Task<Result<Role?>> GetById(int id)
        {
            if (id < 0 || id > Enum.GetNames(typeof(RoleType)).Length)
                return Result<Role?>.Fail("id is out of valid rage");
            return Result<Role?>.Ok(await _roleRepository.GetById(id));
        }
    }
}
