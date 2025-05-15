using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class ComponentConfiguration : IEntityTypeConfiguration<ComponentEntity>
    {
        public void Configure(EntityTypeBuilder<ComponentEntity> builder)
        {
            builder.HasKey(x => x.SerialNumber);

            builder.Property(x => x.AcquistionDate).IsRequired();

            builder.Property(x => x.ModelId).IsRequired();


            builder
                .HasOne(c => c.Model)
                .WithMany(m => m.Components)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c => c.Prop)
                .WithMany(p => p.Components)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
