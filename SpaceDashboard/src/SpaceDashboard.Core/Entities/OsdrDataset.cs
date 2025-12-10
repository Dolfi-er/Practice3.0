namespace SpaceDashboard.Core.Entities;

public class OsdrDataset
{
    public Guid Id { get; set; }
    public string DatasetId { get; set; } = null!;
    public string? Title { get; set; }
    public string? Status { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime InsertedAt { get; set; }
    public string? RestUrl { get; set; }
    public string? RawData { get; set; } // JSON
}