using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class ComponentModelConfiguration : IEntityTypeConfiguration<ComponentModelEntity>
    {
        public void Configure(EntityTypeBuilder<ComponentModelEntity> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.ModelName)
                .IsRequired()
                .HasMaxLength(ComponentModel.MAX_MODEL_NAME_LENGTH);

            builder.Property(m => m.TypeId).IsRequired();

            builder.Property(m => m.KindId).IsRequired();

            builder
                .HasOne(m => m.Type)
                .WithMany(t => t.ComponentModels)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(m => m.Kind)
                .WithMany(k => k.ComponentModels)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(m => m.Components)
                .WithOne(c => c.Model)
                .HasForeignKey(c => c.ModelId);
        }
    }
}
