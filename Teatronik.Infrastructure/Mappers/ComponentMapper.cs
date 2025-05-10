using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class ComponentMapper
    {
        public static Component? ToModel(ComponentEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = Component.Initialize(entity.SerialNumber, entity.AcquistionDate, entity.ModelId, entity.PropId);
            return cm.IsSuccess ? cm.Value : null;
        }

        public static ComponentEntity ToEntity(Component component)
        {
            return new ComponentEntity
            {
                SerialNumber = component.SerialNumber,
                AcquistionDate = component.AcquisitionDate,
                ModelId = component.ModelId,
                PropId = component.PropId
            };
        }
    }
}
