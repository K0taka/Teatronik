using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(User.MAX_NAME_LENGTH);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(User.MAX_EMAIL_LENGTH);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(User.MAX_PASSWORD_HASH);

            builder.Property(u => u.RegistrationDate).IsRequired();

            builder
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users);
        }
    }
}
