using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class Prop
    {
        public const int MAX_PROP_NAME_LENGTH = 250;

        public Guid Id { get; }
        public string PropName { get; private set; }
        public DateOnly Created { get; }
        public Guid SchemaId { get; }

        private Prop(Guid id, string propName, DateOnly created, Guid schemaId)
        {
            Id = id;
            PropName = propName;
            Created = created;
            SchemaId = schemaId;
        }

        private Prop(string propName, DateOnly created, Guid schemaId) : this(Guid.NewGuid(), propName, created, schemaId) {}

        public static Result<Prop> Create(string propName, DateOnly created, Guid schemaId) => Initialize(Guid.NewGuid(), propName, created, schemaId);

        public static Result<Prop> Initialize(Guid id, string propName, DateOnly created, Guid schemaId)
        {
            if (Guid.Empty.Equals(id))
                return Result<Prop>.Fail("Id must be not empty");
            if (string.IsNullOrWhiteSpace(propName))
                return Result<Prop>.Fail("Prop name must be not empty");
            if (propName.Length > MAX_PROP_NAME_LENGTH)
                return Result<Prop>.Fail($"Prop name must be not greater than {MAX_PROP_NAME_LENGTH}");
            if (DateOnly.FromDateTime(DateTime.Now).CompareTo(created) < 0)
                return Result<Prop>.Fail("Creation date must be not later than today");
            if (schemaId.Equals(Guid.Empty))
                return Result<Prop>.Fail("Schema Id must be not empty");

            return Result<Prop>.Ok(new(id, propName, created, schemaId));
        }

        public Result UpdateName(string propName)
        {
            if (string.IsNullOrWhiteSpace(propName))
                return Result.Fail("Prop name must be not empty");
            if (propName.Length > MAX_PROP_NAME_LENGTH)
                return Result.Fail($"Prop name must be not greater than {MAX_PROP_NAME_LENGTH}");

            PropName = propName;
            return Result.Ok();
        }
    }
}
