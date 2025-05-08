using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class TypeConfiguration : IEntityTypeConfiguration<TypeEntity>
    {
        public void Configure(EntityTypeBuilder<TypeEntity> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TypeName).IsRequired();

            builder
                .HasMany(t => t.ComponentModels)
                .WithOne(m => m.Type)
                .HasForeignKey(m => m.TypeId);
        }
    }
}
