using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Kind
    {
        public const int MAX_KIND_NAME_LENGTH = 100;

        public Guid Id { get; }
        public string KindName { get; private set; }

        private Kind(Guid id, string kindName)
        {
            Id = id;
            KindName = kindName;
        }

        public Kind(string kindName) : this(Guid.NewGuid(), kindName) {}

        public static Result<Kind> Create(string kindName)
        {
            if (string.IsNullOrWhiteSpace(kindName))
                return Result<Kind>.Fail("Kind name must be not empty");
            if (kindName.Length > MAX_KIND_NAME_LENGTH)
                return Result<Kind>.Fail($"Kind name must be not graeter than {MAX_KIND_NAME_LENGTH}");

            return Result<Kind>.Ok(new(kindName));
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Kind name must be not empty");
            if (name.Length > MAX_KIND_NAME_LENGTH)
                return Result.Fail($"Kind name must be not graeter than {MAX_KIND_NAME_LENGTH}");
            KindName = name;
            return Result.Ok();
        }
    }
}
