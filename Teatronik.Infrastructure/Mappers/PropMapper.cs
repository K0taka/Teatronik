using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class PropMapper
    {
        public static Prop? ToModel(PropEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = Prop.Initialize(entity.Id, entity.PropName, entity.Created, entity.SchemaId);
            return cm.IsSuccess ? cm.Value : null;
        }

        public static PropEntity ToEntity(Prop prop)
        {
            return new PropEntity
            {
                Created = prop.Created,
                Id = prop.Id,
                PropName = prop.PropName,
                SchemaId = prop.SchemaId,
            };
        }
    }
}
