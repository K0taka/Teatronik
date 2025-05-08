using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Type
    {
        public const int MAX_TYPE_NAME_LENGTH = 100;
        public Guid Id { get; }
        public string TypeName { get; private set; }

        private Type(Guid id, string typeName)
        {
            Id = id;
            TypeName = typeName;
        }

        public Type(string typeName) : this(Guid.NewGuid(), typeName) { }

        public static Result<Type> Create(string typeName) => Initialize(Guid.NewGuid(), typeName);

        public static Result<Type> Initialize(Guid id, string typeName)
        {
            if (Guid.Empty.Equals(id))
                return Result<Type>.Fail("Id must be initialized");
            if (string.IsNullOrWhiteSpace(typeName))
                return Result<Type>.Fail("Type name must be not empty");
            if (typeName.Length > MAX_TYPE_NAME_LENGTH)
                return Result<Type>.Fail($"Type name must be not greater than {MAX_TYPE_NAME_LENGTH}");
            return Result<Type>.Ok(new(id, typeName));
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Type name must be not empty");
            if (name.Length > MAX_TYPE_NAME_LENGTH)
                return Result.Fail($"Type name must be not greater than {MAX_TYPE_NAME_LENGTH}");
            TypeName = name;
            return Result.Ok();
        }
    }
}
