using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Season
    {
        public Guid Id { get; }
        public string SeasonName { get; private set; }

        private Season(Guid id, string seasonName)
        {
            Id = id;
            SeasonName = seasonName;
        }

        public Season(string seasonName) : this(Guid.NewGuid(), seasonName) { }

        public static Result<Season> Create(string seasonName)
        {
            if (string.IsNullOrWhiteSpace(seasonName))
                return Result<Season>.Fail("Season name must be not empty");
            return Result<Season>.Ok(new(seasonName));
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Season name must be not empty");
            SeasonName = name;
            return Result.Ok();
        }
    }
}
