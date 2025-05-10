using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class PropSchemaMapper
    {
        public static PropSchema? ToModel(PropSchemaEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = PropSchema.Initialize(entity.Id, entity.SchemaName, entity.Length, entity.Width, entity.Height);
            return cm.IsSuccess ? cm.Value : null;
        }

        public static PropSchemaEntity ToEntity(PropSchema prop)
        {
            return new PropSchemaEntity
            {
                Id = prop.Id,
                SchemaName = prop.SchemaName,
                Height = prop.Height,
                Width = prop.Width,
                Length = prop.Length
            };
        }
    }
}
