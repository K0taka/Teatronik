using System.Text.Json.Serialization;
using Teatronik.Application;

namespace Teatronik.API.Contracts
{
    public record UpdatePropRequest(
        string PropName);
}