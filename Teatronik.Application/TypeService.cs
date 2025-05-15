using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;

namespace Teatronik.Application
{
    public class TypeService : ITypeService
    {
        private readonly ITypeRepository _typeRepository;

        public TypeService(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<Result<List<Core.Models.Type>>> GetAllAsync() =>
            Result<List<Core.Models.Type>>.Ok(await _typeRepository.GetAllAsync());

        public async Task<Result<Core.Models.Type?>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result<Core.Models.Type?>.Fail("id was empty");
            return Result<Core.Models.Type?>.Ok(await _typeRepository.GetByIdAsync(id));
        }

        public async Task<Result<List<Core.Models.Type>>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result<List<Core.Models.Type>>.Fail("name was empty");
            return Result<List<Core.Models.Type>>.Ok(await _typeRepository.GetByNameAsync(name));
        }

        public async Task<Result> AddAsync(Core.Models.Type type)
        {
            if (type == null)
                return Result.Fail("type was null");
            await _typeRepository.AddAsync(type);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Fail("id was empty");
            await _typeRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Core.Models.Type type)
        {
            if (type == null)
                return Result.Fail("type was null");
            await _typeRepository.UpdateAsync(type);
            return Result.Ok();
        }
    }
}
