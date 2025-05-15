using System.Text.Json.Serialization;
using Teatronik.Application;

namespace Teatronik.API.Contracts
{
    public record PropResponse(
        Guid Id,
        string PropName,
        [property: JsonConverter(typeof(DateOnlyJsonConverter))]
        DateOnly Created,
        Guid SchemaId
        );
}
