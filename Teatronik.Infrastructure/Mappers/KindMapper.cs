using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class KindMapper
    {
        public static Kind? ToModel(KindEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = Kind.Initialize(entity.Id, entity.KindName);
            return cm.IsSuccess ? cm.Value : null;
        }

        public static KindEntity ToEntity(Kind kind)
        {
            return new KindEntity
            {
                Id = kind.Id,
                KindName = kind.KindName
            };
        }
    }
}
