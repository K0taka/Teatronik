using System.Text.Json.Serialization;
using Teatronik.Core.Enums;

namespace Teatronik.API.Contracts
{
    public record RoleResponse(
        int Id,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        RoleType RoleType
        );
}
