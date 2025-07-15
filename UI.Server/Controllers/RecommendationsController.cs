using LaPelicula.UI.Server.Models;
using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LaPelicula.UI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController(
    IHttpContextAccessor _httpContextAccessor, IRecommendationService _recommendationService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("my_prefs", out string encodedStr))
            return NoContent();

        try
        {
            return Ok(await _recommendationService.RecommendAsync(encodedStr, 25));
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { status = "error", message = "Model is not trained yet", ex = ex.Message } );
        }
    }
}