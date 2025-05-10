using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class EventMapper
    {
        public static Event? ToModel(EventEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = Event.Initialize(
                entity.Id,
                entity.EventName,
                entity.DateTime,
                entity.Duration,
                entity.SeasonId,
                entity.Theme,
                entity.Spectators
                );
            return cm.IsSuccess ? cm.Value : null;
        }
    }
}
