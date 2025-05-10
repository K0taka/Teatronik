using Teatronik.Core.Models;
using Teatronik.Infrastructure.Entities;

namespace Teatronik.Infrastructure.Mappers
{
    public static class SeasonMapper
    {
        public static Season? ToModel(SeasonEntity? entity)
        {
            if (entity == null)
                return null;

            var cm = Season.Initialize(entity.Id, entity.SeasonName);
            return cm.IsSuccess ? cm.Value : null;
        }

        public static SeasonEntity ToEntity(Season season)
        {
            return new SeasonEntity
            {
                Id = season.Id,
                SeasonName = season.SeasonName
            };
        }
    }
}
