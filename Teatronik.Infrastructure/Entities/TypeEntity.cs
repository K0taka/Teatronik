namespace Teatronik.Infrastructure.Entities
{
    public class TypeEntity
    {
        public required Guid Id { get; set; }
        public required string TypeName { get; set; }
        public ICollection<ComponentModelEntity> ComponentModels { get; set; } = [];
    }
}
