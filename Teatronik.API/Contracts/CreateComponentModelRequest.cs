namespace Teatronik.API.Contracts
{
    public record CreateComponentModelRequest(
        string ModelName,
        Guid TypeId,
        Guid KindId);
}