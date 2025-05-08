using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class ComponentModelConfiguration : IEntityTypeConfiguration<ComponentModelEntity>
    {
        public void Configure(EntityTypeBuilder<ComponentModelEntity> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.ModelName).IsRequired();

            builder.Property(m => m.TypeId).IsRequired();

            builder.Property(m => m.KindId).IsRequired();

            builder
                .HasOne(m => m.Type)
                .WithMany(t => t.ComponentModels);

            builder
                .HasOne(m => m.Kind)
                .WithMany(k => k.ComponentModels);

            builder
                .HasMany(m => m.Components)
                .WithOne(c => c.Model)
                .HasForeignKey(c => c.ModelId);
        }
    }
}
