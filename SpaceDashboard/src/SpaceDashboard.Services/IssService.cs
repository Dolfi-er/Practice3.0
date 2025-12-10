namespace SpaceDashboard.Services;

public class IssService : IIssService
{
    private readonly IIssRepository _issRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IssService> _logger;
    private readonly IDistributedCache _cache;

    public IssService(
        IIssRepository issRepository,
        IHttpClientFactory httpClientFactory,
        ILogger<IssService> logger,
        IDistributedCache cache)
    {
        _issRepository = issRepository;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _cache = cache;
    }

    public async Task<IssPositionDto> GetCurrentPositionAsync(
        CancellationToken cancellationToken)
    {
        // Пробуем взять из кэша
        var cacheKey = "iss:current";
        var cached = await _cache.GetStringAsync(cacheKey, cancellationToken);
        
        if (!string.IsNullOrEmpty(cached))
        {
            return JsonSerializer.Deserialize<IssPositionDto>(cached)!;
        }

        // Если нет в кэше - из БД
        var position = await _issRepository.GetLatestAsync(cancellationToken);
        
        if (position == null)
        {
            // Если в БД нет - запрашиваем у внешнего API
            position = await FetchFromExternalApiAsync(cancellationToken);
            if (position != null)
            {
                await _issRepository.AddPositionAsync(position, cancellationToken);
            }
        }

        var dto = MapToDto(position);
        
        // Кэшируем на 30 секунд
        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(dto),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            },
            cancellationToken);

        return dto;
    }

    private async Task<IssPosition?> FetchFromExternalApiAsync(
        CancellationToken cancellationToken)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("WhereTheIssAt");
            var response = await client.GetAsync(
                "v1/satellites/25544", 
                cancellationToken);
            
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var data = JsonSerializer.Deserialize<ExternalIssResponse>(json);
            
            return new IssPosition
            {
                Id = Guid.NewGuid(),
                FetchedAt = DateTime.UtcNow,
                Latitude = data?.Latitude ?? 0,
                Longitude = data?.Longitude ?? 0,
                Altitude = data?.Altitude ?? 0,
                Velocity = data?.Velocity ?? 0,
                SourceUrl = "https://api.wheretheiss.at/v1/satellites/25544",
                Payload = json
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при запросе данных ISS");
            return null;
        }
    }
}