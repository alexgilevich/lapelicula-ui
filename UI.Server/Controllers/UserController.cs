using System.Text.Json;
using LaPelicula.UI.Server.Models;
using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Mvc;

namespace LaPelicula.UI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserPreferencesEncoder userPreferencesEncoder, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet("genrePreferences")]
    public ActionResult<GenrePreferences> GetGenrePreferences()
    {
        if (!Request.Cookies.TryGetValue(UserConstants.PreferencesCookieName, out var encodedStr))
            return Ok(new GenrePreferences());

        var preferences = userPreferencesEncoder.Decode(encodedStr);
        
        return Ok(preferences);
    }

    [HttpPost("genrePreferences")]
    public IActionResult SaveGenrePreferences([FromBody] GenrePreferences preferences)
    {
        // for now we will just store the preferences in a cookie, but in the future we can associate it with a user profile in the database
        var encoded = userPreferencesEncoder.Encode(preferences);
        
        Response.Cookies.Append(UserConstants.PreferencesCookieName, encoded, new CookieOptions
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
        });

        return Ok();
    }
}
