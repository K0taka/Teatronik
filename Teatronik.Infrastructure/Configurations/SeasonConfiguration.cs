using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<SeasonEntity>
    {
        public void Configure(EntityTypeBuilder<SeasonEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(s => s.SeasonName).IsRequired();

            builder
                .HasMany(s => s.Events)
                .WithOne(e => e.Season)
                .HasForeignKey(e => e.SeasonId);
        }
    }
}
