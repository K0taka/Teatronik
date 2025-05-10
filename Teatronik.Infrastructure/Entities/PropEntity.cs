namespace Teatronik.Infrastructure.Entities
{
    public class PropEntity
    {
        public required Guid Id { get; set; }
        public required string PropName { get; set; }
        public required DateOnly Created { get; set; }
        public required Guid SchemaId { get; set; }
        public PropSchemaEntity Schema { get; set; }
        public ICollection<EventEntity> Events { get; set; } = [];
        public ICollection<ComponentEntity> Components { get; set; } = [];
    }
}
