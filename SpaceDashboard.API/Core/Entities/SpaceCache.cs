namespace SpaceDashboard.API.Core.Entities;
public class SpaceCache : BaseEntity
{
    public required string Source { get; set; } // 'apod', 'neo', 'flr', 'cme', 'spacex'
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public required string Payload { get; set; } // JSON
}