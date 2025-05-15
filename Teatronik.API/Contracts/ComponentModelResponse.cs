namespace Teatronik.API.Contracts
{
    public record ComponentModelResponse(
        Guid Id,
        string ModelName, 
        Guid TypeId,
        Guid KindId
        );
}
