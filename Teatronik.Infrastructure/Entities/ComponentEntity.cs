namespace Teatronik.Infrastructure.Entities
{
    public class ComponentEntity
    {
        public required string SerialNumber { get; set; }
        public required DateOnly AcquistionDate { get; set; }
        public required Guid ModelId { get; set; }
        public ComponentModelEntity Model { get; set; }
        public Guid? PropId { get; set; }
        public PropEntity? Prop { get; set; }
    }
}
