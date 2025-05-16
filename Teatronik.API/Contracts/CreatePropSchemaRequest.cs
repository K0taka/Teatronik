namespace Teatronik.API.Contracts
{
    public record CreatePropSchemaRequest(
        string SchemaName,
        float Length,
        float Width,
        float Height);
}