using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class SeasonService : ISeasonService
    {
        private readonly ISeasonRepository _seasonRepository;

        public SeasonService(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }

        public async Task<Result<List<Season>>> GetAllAsync() =>
            Result<List<Season>>.Ok(await _seasonRepository.GetAllAsync());

        public async Task<Result<Season?>> GetByIdAsynk(Guid id)
        {
            if (id == Guid.Empty)
                return Result<Season?>.Fail("id is empty");
            return Result<Season?>.Ok(await _seasonRepository.GetByIdAsync(id));
        }

        public async Task<Result<List<Season>>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result<List<Season>>.Fail("name was empty");
            return Result<List<Season>>.Ok(await _seasonRepository.GetByNameAsync(name));
        }

        public async Task<Result> AddAsync(Season season)
        {
            if (season == null)
                return Result.Fail("Season was null");
            await _seasonRepository.AddAsync(season);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Fail("id was empty");
            await _seasonRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Season season)
        {
            if (season == null)
                return Result.Fail("Season was null");
            await _seasonRepository.UpdateAsync(season);
            return Result.Ok();
        }
    }
}
