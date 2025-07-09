using LaPelicula.UI.Models;
using LaPelicula.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LaPelicula.UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreferencesController(IUserPreferencesService _userPreferencesService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_userPreferencesService.Get());
    }
    
    [HttpPut]
    public IActionResult Apply(UserPreferences userPreferences)
    {
        _userPreferencesService.Apply(userPreferences);
        return Ok();
    }
}