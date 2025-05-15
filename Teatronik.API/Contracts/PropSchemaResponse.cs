namespace Teatronik.API.Contracts
{
    public record PropSchemaResponse(
        Guid Id,
        string SchemaName,
        float Length,
        float Width,
        float Height
        );
}
