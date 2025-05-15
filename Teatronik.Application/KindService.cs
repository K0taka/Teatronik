using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class KindService : IKindService
    {
        private readonly IKindRepository _kindRepository;

        public KindService(IKindRepository kindRepository)
        {
            _kindRepository = kindRepository;
        }

        public async Task<Result<List<Kind>>> GetAllAsync() =>
            Result<List<Kind>>.Ok(await _kindRepository.GetAllAsync());

        public async Task<Result<Kind?>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result<Kind?>.Fail("id was empty");
            return Result<Kind?>.Ok(await _kindRepository.GetByIdAsync(id));
        }

        public async Task<Result<List<Kind>>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<List<Kind>>.Fail("Name is empty");
            return Result<List<Kind>>.Ok(await _kindRepository.GetByNameAsync(name));
        }

        public async Task<Result> AddAsync(Kind kind)
        {
            if (kind == null)
                return Result.Fail("Kind was null");
            await _kindRepository.AddAsync(kind);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Fail ($"{nameof(Kind)} cannot be deleted");
            await _kindRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Kind kind)
        {
            if (kind == null)
                return Result.Fail("Kind is null");
            await _kindRepository.UpdateAsync(kind);
            return Result.Ok();
        }

    }
}
