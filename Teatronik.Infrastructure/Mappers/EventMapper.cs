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
            if (cm.IsSuccess)
                foreach (var x in entity.Props)
                {
                    var propModel = PropMapper.ToModel(x);
                    if (propModel != null )
                        cm.Value!.AddProp(propModel);
                }
            return cm.IsSuccess ? cm.Value : null;
        }

        public static EventEntity ToEntity(Event ev)
        {
            return new EventEntity
            {
                Id = ev.Id,
                DateTime = ev.DateTime,
                SeasonId = ev.SeasonId,
                Theme = ev.Theme,
                Spectators = ev.Spectators,
                Duration = ev.Duration,
                EventName = ev.EventName,
                Props = ev.Props.Select(PropMapper.ToEntity).ToList()
            };
        }
    }
}
