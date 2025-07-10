using LaPelicula.UI.Server.Models;
using LaPelicula.UI.Server.Services;
using LaPelicula.UI.Shared;
using Microsoft.AspNetCore.Mvc;

namespace LaPelicula.UI.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreferencesController(IUserPreferencesEncoder userPreferencesEncoder) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("my_prefs", out string preferencesEncodedStr))
            return preferences;
        return Ok(userPreferencesEncoder.Get());
    }
    
    [HttpPut]
    public IActionResult Apply(UserPreferences userPreferences)
    {
        
        var cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTimeOffset.Now.AddDays(183);
        
        _httpContextAccessor.HttpContext.Response.Cookies.Append("my_prefs", base64Encoded, cookieOptions);
        userPreferencesEncoder.Apply(userPreferences);
        return Ok();
    }
}