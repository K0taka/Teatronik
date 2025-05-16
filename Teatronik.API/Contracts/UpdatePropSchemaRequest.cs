namespace Teatronik.API.Contracts
{
    public record UpdatePropSchemaRequest(
        string SchemaName,
        float Length,
        float Width,
        float Height);
}