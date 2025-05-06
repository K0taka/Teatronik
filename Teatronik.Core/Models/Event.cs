using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Event
    {
        public Guid Id { get; }
        public string EventName { get; private set; }
        public DateTime DateTime { get; private set; }
        public int Duration { get; private set; }
        public Guid SeasonId { get; private set; }
        public string Theme { get; private set; }
        public int Spectators { get; private set; }
        
        private Event(
            Guid id,
            string eventName,
            DateTime dateTime,
            int duration,
            Guid seasonId,
            string? theme = null,
            int? spectators = null
            )
        {
            Id = id;
            EventName = eventName;
            DateTime = dateTime;
            Duration = duration;
            SeasonId = seasonId;
            Theme = theme ?? string.Empty;
            Spectators = spectators ?? 0;
        }

        private Event(
            string eventName,
            DateTime dateTime,
            int duration,
            Guid seasonId,
            string? theme = null,
            int? spectators = null
            ) : this(Guid.NewGuid(), eventName, dateTime, duration, seasonId, theme, spectators)
        { }

        public static Result<Event> Create(
            string eventName,
            DateTime dateTime,
            int duration,
            Guid seasonId,
            string? theme = null,
            int? spectators = null
            )
        {
            if (string.IsNullOrWhiteSpace(eventName))
                return Result<Event>.Fail("Event name must be not empty");

            if (duration <= 0)
                return Result<Event>.Fail("Duration of event is a positive integer number of minutes");

            if (seasonId.Equals(Guid.Empty))
                return Result<Event>.Fail("SeasonId must be not empty");

            if (spectators != null && spectators < 0)
                return Result<Event>.Fail("Spectatorn must be non-negative number");

            return Result<Event>.Ok(new(eventName, dateTime, duration, seasonId, theme, spectators));
        }

        public Result UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Fail("Name must be not empty");
            EventName = newName;
            return Result.Ok();
        }

        public Result UpdateDateTime(DateTime dateTime)
        {
            DateTime = dateTime;
            return Result.Ok();
        }

        public Result UpdateDuration(int duration)
        {
            if (duration <= 0)
                return Result.Fail("Duration of event is a positive integer number of minutes");
            Duration = duration;
            return Result.Ok();
        }

        public Result UpdateSeasonId(Guid seasonId)
        {
            if (seasonId.Equals(Guid.Empty))
                return Result.Fail("SeasonId must be not empty");
            SeasonId = seasonId;
            return Result.Ok();
        }

        public Result UpdateTheme(string theme)
        {
            Theme = theme;
            return Result.Ok();
        }

        public Result UpdateSpectators(int spectators)
        {
            if (spectators <= 0)
                return Result.Fail("Spectators must be non-negative number");
            Spectators = spectators;
            return Result.Ok();
        }

    }
}
