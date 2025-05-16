using System.Text.Json.Serialization;
using Teatronik.Application;

namespace Teatronik.API.Contracts
{
    // DTO classes
    public record CreatePropRequest(
        string PropName,
        [property: JsonConverter(typeof(DateOnlyJsonConverter))] DateOnly Created,
        Guid SchemaId);
}