namespace SpaceDashboard.Core.Entities;

public class TelemetryLegacy
{
    public Guid Id { get; set; }
    public DateTime RecordedAt { get; set; }
    public decimal Voltage { get; set; }
    public decimal Temperature { get; set; }
    public string SourceFile { get; set; } = null!;
}