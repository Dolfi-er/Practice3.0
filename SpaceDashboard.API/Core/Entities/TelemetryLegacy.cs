namespace SpaceDashboard.API.Core.Entities;
public class TelemetryLegacy : BaseEntity
{
    public DateTime RecordedAt { get; set; }
    public decimal Voltage { get; set; }
    public decimal Temperature { get; set; }
    public required string SourceFile { get; set; }
}