namespace SpaceDashboard.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class IssController : ControllerBase
{
    private readonly IIssService _issService;
    private readonly ILogger<IssController> _logger;

    public IssController(
        IIssService issService, 
        ILogger<IssController> logger)
    {
        _issService = issService;
        _logger = logger;
    }

    [HttpGet("current")]
    [ProducesResponseType(typeof(ApiResponse<IssPositionDto>), 200)]
    [ProducesResponseType(typeof(ApiErrorResponse), 400)]
    public async Task<IActionResult> GetCurrentPosition()
    {
        try
        {
            var position = await _issService.GetCurrentPositionAsync();
            return Ok(new ApiResponse<IssPositionDto>
            {
                Ok = true,
                Data = position,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения данных ISS");
            return BadRequest(new ApiErrorResponse
            {
                Ok = false,
                Error = new ErrorDetail
                {
                    Code = "ISS_FETCH_ERROR",
                    Message = "Не удалось получить данные о положении МКС",
                    TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            });
        }
    }

    [HttpGet("trend")]
    public async Task<IActionResult> GetTrend(
        [FromQuery] int limit = 240)
    {
        // Логика расчёта тренда как в Rust-сервисе
    }
}