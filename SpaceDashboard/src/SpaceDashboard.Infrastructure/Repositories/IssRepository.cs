namespace SpaceDashboard.Infrastructure.Repositories;

public class IssRepository : IIssRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<IssRepository> _logger;

    public IssRepository(AppDbContext context, ILogger<IssRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IssPosition?> GetLatestAsync(CancellationToken cancellationToken)
    {
        return await _context.IssPositions
            .OrderByDescending(x => x.FetchedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<IssPosition>> GetPositionsAsync(
        int limit, 
        CancellationToken cancellationToken)
    {
        return await _context.IssPositions
            .OrderByDescending(x => x.FetchedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }

    public async Task AddPositionAsync(
        IssPosition position, 
        CancellationToken cancellationToken)
    {
        await _context.IssPositions.AddAsync(position, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}