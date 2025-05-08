using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName).IsRequired();

            builder.Property(u => u.Email).IsRequired();

            builder.Property(u => u.PasswordHash).IsRequired();

            builder.Property(u => u.RegistrationDate).IsRequired();

            builder
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users);
        }
    }
}
