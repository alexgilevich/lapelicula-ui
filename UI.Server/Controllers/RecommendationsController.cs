using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LaPelicula.UI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController(
    IHttpContextAccessor _httpContextAccessor, IRecommendationService _recommendationService, IUserPreferencesEncoder _userPreferencesEncoder, ILogger<RecommendationsController> _logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (!_httpContextAccessor.HttpContext!.Request.Cookies.TryGetValue(UserConstants.PreferencesCookieName, out string? encodedStr))
            return NoContent();

        try
        {
            var genrePreferences = _userPreferencesEncoder.Decode(encodedStr);
            var userPreferences = new UI.Shared.UserPreferences
            {
                Action = genrePreferences.Action,
                Adventure = genrePreferences.Adventure,
                Animation = genrePreferences.Animation,
                Comedy = genrePreferences.Comedy,
                Crime = genrePreferences.Crime,
                Documentary = genrePreferences.Documentary,
                Drama = genrePreferences.Drama,
                Fantasy = genrePreferences.Fantasy,
                FilmNoir = genrePreferences.FilmNoir,
                Horror = genrePreferences.Horror,
                Kids = genrePreferences.Kids,
                Musical = genrePreferences.Musical,
                Mystery = genrePreferences.Mystery,
                Romance = genrePreferences.Romance,
                Scifi = genrePreferences.Scifi,
                Thriller = genrePreferences.Thriller,
                War = genrePreferences.War,
                Western = genrePreferences.Western 
            };
            var recommendations = await _recommendationService.RecommendAsync(userPreferences, 200);
            return Ok(recommendations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching recommendations");
            return StatusCode(503, new { status = "error", message = "Model is not trained yet", ex = ex.Message } );
        }
    }
}