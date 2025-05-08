using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class PropSchema
    {
        public const int MAX_PROP_SCHEMA_NAME_LENGTH = 250;
        public Guid Id { get; }
        public string SchemaName { get; private set; }
        public float Length { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        private PropSchema(Guid id, string schemaName, float length, float width, float height)
        {
            Id = id;
            SchemaName = schemaName;
            Length = length;
            Width = width;
            Height = height;
        }

        private PropSchema(string schemaName, float length, float width, float height) :
            this(Guid.NewGuid(), schemaName, length, width, height)
        { }

        public static Result<PropSchema> Create(string schemaName, float length, float width, float height)
        {
            if (string.IsNullOrWhiteSpace(schemaName))
                return Result<PropSchema>.Fail("Schema name must be not empty");

            if (schemaName.Length > MAX_PROP_SCHEMA_NAME_LENGTH)
                return Result<PropSchema>.Fail($"Schema name length must be not greater thab {MAX_PROP_SCHEMA_NAME_LENGTH}");

            if (length <= 0)
                return Result<PropSchema>.Fail("Length must be positive");

            if (width <= 0)
                return Result<PropSchema>.Fail("Width must be positive");

            if (height <= 0)
                return Result<PropSchema>.Fail("Height must be positive");

            return Result<PropSchema>.Ok(new(schemaName, length, width, height));
        }

        public Result UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Fail("Schema name must be not empty");
            if (name.Length > MAX_PROP_SCHEMA_NAME_LENGTH)
                return Result.Fail($"Schema name length must be not greater thab {MAX_PROP_SCHEMA_NAME_LENGTH}");
            SchemaName = name;
            return Result.Ok();
        }

        public Result UpdateLength(float length)
        {
            if (length <= 0)
                return Result<PropSchema>.Fail("Length must be positive");
            Length = length;
            return Result.Ok();
        }

        public Result UpdateWidth(float width)
        {
            if (width <= 0)
                return Result<PropSchema>.Fail("Width must be positive");
            Width = width;
            return Result.Ok();
        }

        public Result UpdateHeight(float height)
        {
            if (height <= 0)
                return Result<PropSchema>.Fail("Height must be positive");
            Height = height;
            return Result.Ok();
        }
    }
}
