namespace Teatronik.API.Contracts
{
    public record CreateEventRequest(
        string EventName,
        DateTime DateTime,
        int Duration,
        Guid SeasonId,
        string Theme,
        int Spectators);
}