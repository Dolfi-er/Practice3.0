namespace SpaceDashboard.API.Core.Entities;

public class CmsBlock : BaseEntity
{
    public required string Slug { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public bool IsActive { get; set; } = true;
}