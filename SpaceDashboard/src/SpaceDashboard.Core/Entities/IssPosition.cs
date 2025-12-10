namespace SpaceDashboard.Core.Entities;

public class IssPosition
{
    public Guid Id { get; set; }
    public DateTime FetchedAt { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double Velocity { get; set; }
    public string? SourceUrl { get; set; }
    public string? Payload { get; set; } // JSON
}
