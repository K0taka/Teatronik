using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Type
    {
        public Guid Id { get; }
        public string TypeName { get; private set; }

        private Type(Guid id, string typeName)
        {
            Id = id;
            TypeName = typeName;
        }

        public Type(string typeName) : this(Guid.NewGuid(), typeName) { }

        public static Result<Type> Create(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return Result<Type>.Fail("Type name must be not empty");
            return Result<Type>.Ok(new(typeName));
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Type name must be not empty");
            TypeName = name;
            return Result.Ok();
        }
    }
}
