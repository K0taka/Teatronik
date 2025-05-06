namespace Teatronik.Core.Models
{
    public class ComponentModel
    {
        public Guid Id { get; }
        public string ModelName { get; private set; }
        public Guid TypeId { get; }
        public Guid KindId { get; }


    }
}
