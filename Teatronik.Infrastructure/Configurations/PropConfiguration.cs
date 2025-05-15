using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class PropConfiguration : IEntityTypeConfiguration<PropEntity>
    {
        public void Configure(EntityTypeBuilder<PropEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PropName)
                .IsRequired()
                .HasMaxLength(Prop.MAX_PROP_NAME_LENGTH);

            builder.Property(p => p.Created).IsRequired();

            builder.Property(p => p.SchemaId).IsRequired();

            builder
                .HasOne(p => p.Schema)
                .WithMany(s => s.Props)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Events)
                .WithMany(e => e.Props);

            builder
                .HasMany(p => p.Components)
                .WithOne(c => c.Prop)
                .HasForeignKey(c => c.PropId);
        }
    }
}
