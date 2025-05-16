using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _componentRepository;

        public ComponentService(IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }

        public async Task<Result<List<Component>>> GetAllAsync() =>
            Result<List<Component>>.Ok(await _componentRepository.GetAllAsync());

        public async Task<Result<Component?>> GetBySerialAsync(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return Result<Component?>.Fail("serial number was empty");

            return Result<Component?>.Ok(await _componentRepository.GetBySerialAsync(serialNumber));
        }

        public async Task<Result<List<Component>>> GetByFilterAsync(
            bool? isUsed = null,
            string? name = null,
            Guid[]? typeIds = null,
            Guid[]? kindIds = null,
            Guid? modelId = null,
            Guid? propId = null
            ) => Result<List<Component>>.Ok(await _componentRepository.GetByFilterAsync(
                isUsed, name, typeIds, kindIds, modelId, propId));

        public async Task<Result> AddAsync(Component component)
        {
            if (component == null)
                return Result.Fail("component was null");
            await _componentRepository.AddAsync(component);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return Result.Fail("Serial number was empty");
            await _componentRepository.DeleteAsync(serialNumber);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(Component component)
        {
            if (component == null)
                return Result.Fail($"{nameof(component)} is not valid");
            await _componentRepository.UpdateAsync(component);
            return Result.Ok();
        }
    }
}
