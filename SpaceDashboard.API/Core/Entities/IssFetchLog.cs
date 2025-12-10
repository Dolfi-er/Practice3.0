namespace SpaceDashboard.API.Core.Entities;

public class IssFetchLog : BaseEntity
{
    public DateTime FetchedAt { get; set; }
    public required string SourseUrl { get; set; }
    public required string Payload { get; set; }
}