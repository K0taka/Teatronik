namespace Teatronik.Infrastructure.Entities
{
    public class ComponentModelEntity
    {
        public required Guid Id { get; set; }
        public required string ModelName { get; set; }
        public required Guid TypeId { get; set; }
        public required TypeEntity Type { get; set; }

        public required Guid KindId { get; set; }
        public required KindEntity Kind { get; set; }

        public ICollection<ComponentEntity> Components { get; set; } = [];
    }
}
