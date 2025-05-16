using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class PropService : IPropService
    {
        private readonly IPropRepository _propRepository;

        public PropService(IPropRepository propRepository)
        {
            _propRepository = propRepository;
        }

        public async Task<Result<List<Prop>>> GetAllAsync() =>
            Result<List<Prop>>.Ok(await _propRepository.GetAllAsync());

        public async Task<Result<Prop?>> GetByIdAsync(Guid id)
        {
            if (Guid.Empty == id)
                return Result<Prop?>.Fail("id was empty");
            return Result<Prop?>.Ok(await _propRepository.GetById(id));
        }

        public async Task<Result<List<Prop>>> GetByFilterAsync(
            bool? isUsed = null,
            string? name = null,
            Guid[]? schemaIds = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null
            ) => Result<List<Prop>>.Ok(await _propRepository.GetByFilterAsync(
                isUsed,
                name,
                schemaIds,
                fromDate,
                toDate
                ));

        public async Task<Result> AddAsync(Prop prop)
        {
            if (prop == null)
                return Result.Fail("prop was null");
            await _propRepository.AddAsync(prop);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Prop prop)
        {
            if (prop == null)
                return Result.Fail("prop was null");
            await _propRepository.UpdateAsync(prop);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Fail("id was empty");
            await _propRepository.DeleteAsync(id);
            return Result.Ok();
        }
    }
}
