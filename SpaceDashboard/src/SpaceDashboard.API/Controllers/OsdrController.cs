[ApiController]
[Route("api/osdr")]
public class OsdrController : ControllerBase
{
    private readonly IOsdrService _osdrService;

    [HttpGet]
    public async Task<IActionResult> GetDatasets(
        [FromQuery] int limit = 20,
        [FromQuery] int page = 1)
    {
        var result = await _osdrService.GetDatasetsAsync(limit, page);
        return Ok(new ApiResponse<PagedResult<OsdrDatasetDto>>
        {
            Ok = true,
            Data = result,
            Timestamp = DateTime.UtcNow
        });
    }
}