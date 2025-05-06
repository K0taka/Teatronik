using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Kind
    {
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
            return Result<Kind>.Ok(new(kindName));
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Kind name must be not empty");
            KindName = name;
            return Result.Ok();
        }
    }
}
