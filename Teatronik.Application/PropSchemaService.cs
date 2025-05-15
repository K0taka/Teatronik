using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class PropSchemaService : IPropSchemaService
    {
        private readonly IPropSchemaRepository _propSchemaRepository;

        public PropSchemaService(IPropSchemaRepository propSchemaRepository)
        {
            _propSchemaRepository = propSchemaRepository;
        }

        public async Task<Result<List<PropSchema>>> GetAllAsync() =>
            Result<List<PropSchema>>.Ok(await _propSchemaRepository.GetAllAsync());

        public async Task<Result<PropSchema?>> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result<PropSchema?>.Fail("id was empty");
            return Result<PropSchema?>.Ok(await _propSchemaRepository.GetByIdAsync(id));
        }

        public async Task<Result<List<PropSchema>>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<List<PropSchema>>.Fail("name was empty");
            return Result<List<PropSchema>>.Ok(await _propSchemaRepository.GetByName(name));
        }

        public async Task<Result> AddAsync(PropSchema schema)
        {
            if (schema == null)
                return Result.Fail("Schema was null");
            await _propSchemaRepository.AddAsync(schema);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return Result.Fail("id was empty");
            await _propSchemaRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(PropSchema schema)
        {
            if (schema == null)
                return Result.Fail("Schema was null");
            await _propSchemaRepository.UpdateAsync(schema);
            return Result.Ok();
        }
    }
}
