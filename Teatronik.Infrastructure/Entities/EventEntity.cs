namespace Teatronik.Infrastructure.Entities
{
    public class EventEntity
    {
        public required Guid Id { get; set; }
        public required string EventName { get; set; }
        public required DateTime DateTime { get; set; }
        public required int Duration { get; set; }
        public string Theme { get; set; } = string.Empty;
        public int Spectators { get; set; }
        public required Guid SeasonId { get; set; }
        public required SeasonEntity Season { get; set; }

        public ICollection<PropEntity> Props { get; set; } = [];
    }
}
