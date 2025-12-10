[ApiController]
[Route("api/cms")]
public class CmsController : ControllerBase
{
    private readonly ICmsService _cmsService;

    [HttpGet("page/{slug}")]
    public async Task<IActionResult> GetPage(string slug)
    {
        var page = await _cmsService.GetPageBySlugAsync(slug);
        
        if (page == null)
            return NotFound(new ApiErrorResponse
            {
                Ok = false,
                Error = new ErrorDetail
                {
                    Code = "PAGE_NOT_FOUND",
                    Message = $"Страница '{slug}' не найдена"
                }
            });

        return Ok(new ApiResponse<CmsPageDto>
        {
            Ok = true,
            Data = page
        });
    }
}