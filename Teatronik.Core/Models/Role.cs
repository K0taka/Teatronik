using Teatronik.Core.Common;
using Teatronik.Core.Enums;

namespace Teatronik.Core.Models
{
    public class Role
    {
        private static int ids = 0;
        public int Id { get; }
        public RoleType RoleType { get; }

        private Role(int id, RoleType roleType)
        {
            Id = id;
            RoleType = roleType;
        }

        public static Result<Role> Create(RoleType roleType)
        {
            return Initialize(ids++, roleType);
        }

        public static Result<Role> Initialize(int id, RoleType roleType)
        {
            return Result<Role>.Ok(new(id, roleType));
        }
    }
}
