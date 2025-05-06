namespace Teatronik.Infrastructure.Entities
{
    public class PropSchemaEntity
    {
        public required Guid Id { get; set; }
        public required string SchemaName { get; set; }
        public required float Length { get; set; }
        public required float Width { get; set; }
        public required float Height { get; set; }
        public ICollection<PropEntity> Props { get; set; } = [];
    }
}
