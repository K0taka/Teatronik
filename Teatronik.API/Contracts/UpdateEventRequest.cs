
namespace Teatronik.API.Contracts
{
    public record UpdateEventRequest(
        string EventName,
        DateTime DateTime,
        int Duration,
        Guid SeasonId,
        string Theme,
        int Spectators);
}