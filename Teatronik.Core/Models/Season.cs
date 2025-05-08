using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Season
    {
        public const int MAX_SEASON_NAME_LENGTH = 200;

        public Guid Id { get; }
        public string SeasonName { get; private set; }

        private Season(Guid id, string seasonName)
        {
            Id = id;
            SeasonName = seasonName;
        }

        public Season(string seasonName) : this(Guid.NewGuid(), seasonName) { }

        public static Result<Season> Create(string seasonName) => Initialize(Guid.NewGuid(), seasonName);

        public static Result<Season> Initialize(Guid id, string seasonName)
        {
            if (Guid.Empty.Equals(id))
                return Result<Season>.Fail("Id must be initialized");
            if (string.IsNullOrWhiteSpace(seasonName))
                return Result<Season>.Fail("Season name must be not empty");
            if (seasonName.Length > MAX_SEASON_NAME_LENGTH)
                return Result<Season>.Fail($"Season name length must be not greater than {MAX_SEASON_NAME_LENGTH}");
            return Result<Season>.Ok(new(id, seasonName));
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Season name must be not empty");
            if (name.Length > MAX_SEASON_NAME_LENGTH)
                return Result.Fail($"Season name length must be not greater than {MAX_SEASON_NAME_LENGTH}");
            SeasonName = name;
            return Result.Ok();
        }
    }
}
