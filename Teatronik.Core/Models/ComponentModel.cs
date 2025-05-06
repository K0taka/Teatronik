using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class ComponentModel
    {
        public Guid Id { get; }
        public string ModelName { get; private set; }
        public Guid TypeId { get; private set; }
        public Guid KindId { get; private set; }

        private ComponentModel(Guid id, string modelName, Guid typeId, Guid kindId)
        {
            Id = id;
            ModelName = modelName;
            TypeId = typeId;
            KindId = kindId;
        }

        private ComponentModel(string modelName, Guid typeId, Guid kindId) :
            this(Guid.NewGuid(), modelName, typeId, kindId) {}

        public static Result<ComponentModel> Create(string modelName, Guid typeId, Guid kindId)
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return Result<ComponentModel>.Fail("model name must be not empty");

            if (typeId.Equals(Guid.Empty))
                return Result<ComponentModel>.Fail("typeId must be not empty");

            if (kindId.Equals(Guid.Empty))
                return Result<ComponentModel>.Fail("kindId must be not empty");

            return Result<ComponentModel>.Ok(new(modelName, typeId, kindId));
        }

        public Result ChangeName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Fail("name must be not empty");
            ModelName = newName;
            return Result.Ok();
        }

        public Result ChangeType(Guid typeId)
        {
            if (typeId.Equals(Guid.Empty))
                return Result.Fail("typeId must be not empty");

            TypeId = typeId;
            return Result.Ok();
        }

        public Result ChangeKind(Guid kindId)
        {
            if (kindId.Equals(Guid.Empty))
                return Result.Fail("kindId must be not empty");

            KindId = kindId;
            return Result.Ok();
        }
    }
}
