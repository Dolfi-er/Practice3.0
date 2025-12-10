namespace SpaceDashboard.API.Core.Entities;

public class OsdrItem : BaseEntity
{
    public string? DatasetId { get; set; }
    public string? Title { get; set; }
    public string? Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime InsertedAt { get; set; } = DateTime.UtcNow;
    public required string Raw { get; set; } // JSON
    public string? RestUrl { get; set; }
}