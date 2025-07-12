using LaPelicula.UI.Server.Models;
using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Mvc;

namespace LaPelicula.UI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController() : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("my_prefs", out string preferencesEncodedStr))
        //     return preferences;
        return Ok(new List<string>() { "movie1", "movie2" });
    }
}