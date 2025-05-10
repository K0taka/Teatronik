using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class ComponentModelMapper
    {
        public static ComponentModel? ToModel(ComponentModelEntity entity)
        {
            if (entity == null)
                return null;

            var cm = ComponentModel.Initialize(entity.Id, entity.ModelName, entity.TypeId, entity.KindId);
            return cm.IsSuccess ? cm.Value : null;
        }

        public static ComponentModelEntity ToEntity(ComponentModel componentModel)
        {
            return new ComponentModelEntity
            { 
                Id = componentModel.Id, 
                KindId = componentModel.KindId, 
                ModelName = componentModel.ModelName,
                TypeId = componentModel.TypeId
            };
        }
    }
}
