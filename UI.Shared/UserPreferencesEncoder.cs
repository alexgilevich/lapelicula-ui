using System.Buffers.Text;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace LaPelicula.UI.Shared;

public interface IUserPreferencesEncoder
{
    UserPreferences Decode(string? preferencesEncodedStr);
    string Encode(UserPreferences preferences);
}

public class UserPreferencesEncoder(ILogger<UserPreferencesEncoder> logger) : IUserPreferencesEncoder
{
    private readonly ILogger<UserPreferencesEncoder> _logger = logger;

    public string Encode(UserPreferences preferences)
    {
        // first convert to dictionary
        var prefDict = preferences.ToDictionary();
        
        // create an array with the genre values in the predefined order
        double[] preferencesArr = UserPreferences.GetAllGenreKeys().Select(key => prefDict.GetValueOrDefault(key, 0.0)).ToArray();

        // convert the array to string
        string preferencesStr = string.Join(",", preferencesArr.Select(g => g.ToString(System.Globalization.CultureInfo.InvariantCulture)));

        // encode the string to UTF8 bytes
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(preferencesStr);

        // convert to Base64 string
        string base64Encoded = Convert.ToBase64String(utf8Bytes);
        return base64Encoded;
    }
    
    public UserPreferences Decode(string? preferencesEncodedStr)
    {
        UserPreferences preferences = new();
        if (string.IsNullOrEmpty(preferencesEncodedStr)) 
            return preferences;
        try
        {
            byte[] base64Bytes = Convert.FromBase64String(preferencesEncodedStr);
            string decodedText = System.Text.Encoding.UTF8.GetString(base64Bytes);
            double[] decodedGenrePreferences = decodedText
                .Split(',')
                .Select(x => Convert.ToDouble(x, new NumberFormatInfo { NumberDecimalSeparator = "." }))
                .ToArray();

            string[] keys = UserPreferences.GetAllGenreKeys();
            var prefDict = decodedGenrePreferences.Select((val, idx) => new KeyValuePair<string, double>(keys[idx], val)).ToDictionary();
            preferences = UserPreferences.FromDictionary(prefDict);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while decoding user preferences");
        }

        return preferences;
    }
}