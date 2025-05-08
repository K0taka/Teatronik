using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class KindConfiguration : IEntityTypeConfiguration<KindEntity>
    {
        public void Configure(EntityTypeBuilder<KindEntity> builder)
        { 
            builder.HasKey(k => k.Id);

            builder.Property(k => k.KindName).IsRequired();

            builder
                .HasMany(k => k.ComponentModels)
                .WithOne(m => m.Kind)
                .HasForeignKey(m => m.KindId);
        }
    }
}
