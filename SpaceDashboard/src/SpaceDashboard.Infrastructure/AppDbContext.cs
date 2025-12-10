namespace SpaceDashboard.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<IssPosition> IssPositions { get; set; }
    public DbSet<OsdrDataset> OsdrDatasets { get; set; }
    public DbSet<CmsPage> CmsPages { get; set; }
    public DbSet<TelemetryLegacy> TelemetryLegacy { get; set; }
    public DbSet<SpaceCache> SpaceCache { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OsdrDataset>()
            .HasIndex(e => e.DatasetId)
            .IsUnique();

        modelBuilder.Entity<CmsPage>()
            .HasIndex(e => e.Slug)
            .IsUnique();

        modelBuilder.Entity<SpaceCache>()
            .HasIndex(e => new { e.Source, e.FetchedAt })
            .IsDescending(true);
    }
}