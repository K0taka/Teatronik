using System.Text.Json.Serialization;
using Teatronik.Core.Enums;

namespace Teatronik.API.Contracts
{
    public record UserResponse(
        Guid Id,
        string FullName,
        string Email,
        DateTime RegistationDate,
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        List<RoleType> Roles
        );
}
