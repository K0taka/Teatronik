using Teatronik.Core.Enums;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class RoleMapper
    {
        public static Role? ToModel(RoleEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = Role.Initialize(entity.Id, Enum.Parse<RoleType>(entity.RoleName));
            return cm.IsSuccess ? cm.Value : null;
        }

        public static RoleEntity ToEntity(Role role)
        {
            return new RoleEntity
            {
                Id = role.Id,
                RoleName = role.RoleType.ToString(),
            };
        }
    }
}
