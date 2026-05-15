using System.Buffers.Text;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LaPelicula.UI.Server.Models;
using Microsoft.Extensions.Logging;

namespace LaPelicula.UI.Server.Services;

public interface IUserPreferencesEncoder
{
    GenrePreferences Decode(string? preferencesEncodedStr);
    string Encode(GenrePreferences preferences);
}

public class UserPreferencesEncoder(ILogger<UserPreferencesEncoder> logger) : IUserPreferencesEncoder
{
    private readonly ILogger<UserPreferencesEncoder> _logger = logger;

    public string Encode(GenrePreferences preferences)
    {
        // serialize preferences to JSON
        string preferencesStr = JsonSerializer.Serialize(preferences, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        });

        // if there are no values we return an empty string instead
        if (!preferencesStr.Contains("\"")) 
            return string.Empty;

        // encode the string to UTF8 bytes
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(preferencesStr);

        // convert to Base64 string
        string base64Encoded = Convert.ToBase64String(utf8Bytes);
        return base64Encoded;
    }
    
    public GenrePreferences Decode(string? preferencesEncodedStr)
    {
        if (string.IsNullOrEmpty(preferencesEncodedStr)) 
            return new GenrePreferences();
        try
        {
            byte[] base64Bytes = Convert.FromBase64String(preferencesEncodedStr);
            string decodedText = Encoding.UTF8.GetString(base64Bytes);
            var decodedGenrePreferences = JsonSerializer.Deserialize<GenrePreferences>(decodedText);
            
            return decodedGenrePreferences ?? new GenrePreferences();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while decoding genre preferences");
            return new GenrePreferences();
        }
    }
}