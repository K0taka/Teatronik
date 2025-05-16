using System.Text.Json.Serialization;
using Teatronik.Application;

namespace Teatronik.API.Contracts
{
    public record CreateComponentRequest(
        string SerialNumber,
        [property: JsonConverter(typeof(DateOnlyJsonConverter))]
        DateOnly AcquisitionDate,
        Guid ModelId,
        Guid? PropId = null);
}