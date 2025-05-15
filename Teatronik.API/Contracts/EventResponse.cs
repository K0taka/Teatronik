namespace Teatronik.API.Contracts
{
    public record EventResponse(
        Guid Id,
        string EventName,
        DateTime DateTime,
        int Duration,
        Guid SeasonId,
        string Theme,
        int Spectators,
        IReadOnlyList<PropResponse> Props
        );
}
