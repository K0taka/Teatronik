namespace Teatronik.Infrastructure.Entities
{
    public class SeasonEntity
    {
        public required Guid Id { get; set; }
        public required string SeasonName { get; set; }

        public ICollection<EventEntity> Events { get; set; } = [];
    }
}
