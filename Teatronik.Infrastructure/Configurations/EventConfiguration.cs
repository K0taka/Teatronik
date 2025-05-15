using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EventName)
                .IsRequired()
                .HasMaxLength(Event.MAX_EVENT_NAME_LENGTH);

            builder.Property(e => e.Theme)
                .HasMaxLength(Event.MAX_EVENT_THEME_LENGTH);

            builder.Property(e => e.DateTime).IsRequired();

            builder.Property(e => e.Duration).IsRequired();

            builder.Property(e => e.SeasonId).IsRequired();

            builder
                .HasOne(e => e.Season)
                .WithMany(s => s.Events)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(e => e.Props)
                .WithMany(p => p.Events);
        }
    }
}
