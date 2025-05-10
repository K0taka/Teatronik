using Teatronik.Core.Enums;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class UserMapper
    {
        public static User? ToModel(UserEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = User.Initialize(entity.Id, entity.FullName, entity.Email, entity.PasswordHash, entity.RegistrationDate);

            if (cm == null || cm.Value == null)
                return null;

            foreach (var role in entity.Roles)
            {
                var isOk = Enum.TryParse(role.RoleName, out RoleType roleType);
                if (isOk)
                    cm.Value.AssignRole(roleType);
            }
            return cm.Value;

        }

        public static UserEntity ToEntity(User user)
        {
            return new UserEntity
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PasswordHash = user.PasswordHash,
                RegistrationDate = user.RegistrationDate
            };
        }
    }
}
