namespace Teatronik.Infrastructure.Entities
{
    public class UserEntity
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required DateTime RegistrationDate { get; set; }

        public ICollection<RoleEntity> Roles { get; set; } = [];
    }
}
