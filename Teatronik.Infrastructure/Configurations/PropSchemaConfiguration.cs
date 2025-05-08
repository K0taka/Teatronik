using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class PropSchemaConfiguration : IEntityTypeConfiguration<PropSchemaEntity>
    {
        public void Configure(EntityTypeBuilder<PropSchemaEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SchemaName).IsRequired();

            builder.Property(s => s.Length).IsRequired();

            builder.Property(s => s.Width).IsRequired();

            builder.Property(s => s.Height).IsRequired();

            builder
                .HasMany(s => s.Props)
                .WithOne(p => p.Schema)
                .HasForeignKey(p => p.SchemaId);
        }
    }
}
