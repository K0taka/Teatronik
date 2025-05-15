using Teatronik.Core.Common;
using Teatronik.Core.Interfaces;
using Teatronik.Core.Models;

namespace Teatronik.Application
{
    public class ComponentModelService : IComponentModelService
    {
        private readonly IComponentModelRepository _componentModelRepository;

        public ComponentModelService(IComponentModelRepository componentModelRepository)
        {
            _componentModelRepository = componentModelRepository;
        }

        public async Task<Result<List<ComponentModel>>> GetAllAsync() =>
            Result<List<ComponentModel>>.Ok( await _componentModelRepository.GetAllAsync() );

        public async Task<Result<ComponentModel?>> GetByIdAsync(Guid id)
        {
            if (Guid.Empty.Equals(id))
                return Result<ComponentModel?>.Fail("Id was empty");
            var componentModel = await _componentModelRepository.GetByIdAsync(id);
            return Result<ComponentModel?>.Ok(componentModel);
        }

        public async Task<Result<List<ComponentModel>>> GetByFilterAsync(
            string? name = null,
            Guid[]? typeIds = null,
            Guid[]? kindIds = null
            ) => Result<List<ComponentModel>>.Ok(await _componentModelRepository.GetByFilterAsync(name, typeIds, kindIds));

        public async Task<Result> AddAsync(ComponentModel componentModel) {
            if (componentModel == null)
                return Result.Fail("componentModel was null");
            await _componentModelRepository.AddAsync(componentModel);
            return Result.Ok();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            if (Guid.Empty.Equals(id))
                return Result.Fail("id was empty");
            await _componentModelRepository.DeleteAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(
            Guid id,
            string modelName,
            Guid kindId,
            Guid typeId
            )
        {
            if (string.IsNullOrWhiteSpace(modelName))
                return Result.Fail("Model name was empty");

            if (Guid.Empty == kindId)
                return Result.Fail("Kind Id was empty");

            if (Guid.Empty == typeId)
                return Result.Fail("Type Id was empty");

            if (Guid.Empty == id)
                return Result.Fail("Id was empty");

            await _componentModelRepository.UpdateAsync(id, modelName, kindId, typeId);

            return Result.Ok();
        }

        public async Task<Result> UpdateAsync(ComponentModel componentModel)
        {
            if (componentModel == null)
                return Result.Fail("component model was empty");
            await _componentModelRepository.UpdateAsync(componentModel);
            return Result.Ok();
        }
    }
}
