namespace Teatronik.Infrastructure.Entities
{
    public class RoleEntity
    {
        public required int Id { get; set; }
        public required string RoleName { get; set; }

        public ICollection<UserEntity> Users { get; set; } = [];
    }
}
