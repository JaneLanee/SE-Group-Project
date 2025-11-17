using Microsoft.AspNetCore.Mvc;

namespace WL_Server.Recommend;

[ApiController]
[Route("api/[controller]")]
public class RecommendController : ControllerBase
{
    private readonly IRecommendService _recommendService;

    public RecommendController(IRecommendService recommendService)
    {
        _recommendService = recommendService;
    }

    [HttpGet("")]
    public IActionResult GetRecommendedList()
    {
        var recList = _recommendService.FetchRecommendedList(2025);

        try
        {
            return Ok(recList);
        }
        catch
        {
            return Ok("Error");
        }
    }
}