using LaPelicula.UI.Server.Services;
using Microsoft.AspNetCore.Mvc;
using UI.Shared;

namespace LaPelicula.UI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController(IMovieRepository movieRepository, ILogger<MovieController> logger) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        try
        {
            var movie = await movieRepository.GetMovieByIdAsync(id);
            return Ok(movie);
        }
        catch (KeyNotFoundException)
        {
            logger.LogWarning("Movie with id {MovieId} not found", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching movie with id {MovieId}", id);
            return StatusCode(500);
        }
    }
}
