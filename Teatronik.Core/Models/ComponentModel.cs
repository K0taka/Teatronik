using Teatronik.Core.Common;

namespace Teatronik.Core.Models
{
    public class ComponentModel
    {
        public const int MAX_MODEL_NAME_LENGTH = 100;

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

        public static Result<ComponentModel> Create(string modelName, Guid typeId, Guid kindId) => 
            Initialize(Guid.NewGuid(), modelName, typeId, kindId);

        public static Result<ComponentModel> Initialize(Guid id, string modelName, Guid typeId, Guid kindId)
        {
            if (Guid.Empty.Equals(id))
                return Result<ComponentModel>.Fail("Model id must be initialized");

            if (string.IsNullOrWhiteSpace(modelName))
                return Result<ComponentModel>.Fail("model name must be not empty");

            if (modelName.Length > MAX_MODEL_NAME_LENGTH)
                return Result<ComponentModel>.Fail($"model name length must be not greater than {MAX_MODEL_NAME_LENGTH}");

            if (typeId.Equals(Guid.Empty))
                return Result<ComponentModel>.Fail("typeId must be not empty");

            if (kindId.Equals(Guid.Empty))
                return Result<ComponentModel>.Fail("kindId must be not empty");

            return Result<ComponentModel>.Ok(new(id, modelName, typeId, kindId));
        }

        public Result UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                return Result.Fail("name must be not empty");
            if (newName.Length > MAX_MODEL_NAME_LENGTH)
                return Result.Fail($"model name length must be not greater than {MAX_MODEL_NAME_LENGTH}");
            ModelName = newName;
            return Result.Ok();
        }

        public Result UpdateType(Guid typeId)
        {
            if (typeId.Equals(Guid.Empty))
                return Result.Fail("typeId must be not empty");

            TypeId = typeId;
            return Result.Ok();
        }

        public Result UpdateKind(Guid kindId)
        {
            if (kindId.Equals(Guid.Empty))
                return Result.Fail("kindId must be not empty");

            KindId = kindId;
            return Result.Ok();
        }
    }
}
