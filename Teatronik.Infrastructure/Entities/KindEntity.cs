namespace Teatronik.Infrastructure.Entities
{
    public class KindEntity
    {
        public required Guid Id { get; set; }
        public required string KindName { get; set; }
        public ICollection<ComponentModelEntity> ComponentModels { get; set; } = [];
    }
}
