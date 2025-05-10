using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class TypeMapper
    {
        public static Core.Models.Type? ToModel(TypeEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = Core.Models.Type.Initialize(entity.Id, entity.TypeName);
            return cm.IsSuccess ? cm.Value : null;
        }

        public static TypeEntity ToEntity(Core.Models.Type type)
        {
            return new TypeEntity
            {
                Id = type.Id,
                TypeName = type.TypeName,
            };
        }
    }
}
