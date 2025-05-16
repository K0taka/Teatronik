namespace Teatronik.API.Contracts
{
    public record UpdateComponentModelRequest(
        string ModelName,
        Guid TypeId,
        Guid KindId);
}