using System.Buffers.Text;
using System.Text;
using LaPelicula.UI.Models;

namespace LaPelicula.UI.Services;

public interface IUserPreferencesService
{
    void Apply(UserPreferences preferences);
    UserPreferences Get();
}

public class UserPreferencesService : IUserPreferencesService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UserPreferencesService> _logger;

    public UserPreferencesService(IHttpContextAccessor httpContextAccessor, ILogger<UserPreferencesService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public void Apply(UserPreferences preferences)
    {
        // Create an array with the genre values in the same order as decoding expects
        double[] preferencesArr = new double[]
        {
            preferences.Action,
            preferences.Adventure,
            preferences.Animation,
            preferences.Kids,
            preferences.Comedy,
            preferences.Crime,
            preferences.Documentary,
            preferences.Drama,
            preferences.Fantasy,
            preferences.Horror,
            preferences.Mystery,
            preferences.Romance,
            preferences.Scifi,
            preferences.Thriller
        };

        // Join the double values as comma-separated string
        string preferencesStr = string.Join(",", preferencesArr.Select(g => g.ToString(System.Globalization.CultureInfo.InvariantCulture)));

        // Encode the string to UTF8 bytes
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(preferencesStr);

        // Convert to Base64 string
        string base64Encoded = Convert.ToBase64String(utf8Bytes);

        var cookieOptions = new CookieOptions();
        cookieOptions.Expires = DateTimeOffset.Now.AddDays(183);
        
        _httpContextAccessor.HttpContext.Response.Cookies.Append("my_prefs", base64Encoded, cookieOptions);
    }
    
    public UserPreferences Get()
    {
        UserPreferences preferences = new UserPreferences();
        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("my_prefs", out string preferencesEncodedStr))
            return preferences;
        
        try
        {
            byte[] base64Bytes = Convert.FromBase64String(preferencesEncodedStr);
            string decodedText = System.Text.Encoding.UTF8.GetString(base64Bytes);
            double[] decodedGenrePreferences = decodedText.Split(',').Select(Convert.ToDouble).ToArray();

            preferences = new UserPreferences
            {
                Action = decodedGenrePreferences[0],
                Adventure = decodedGenrePreferences[1],
                Animation = decodedGenrePreferences[2],
                Kids = decodedGenrePreferences[3],
                Comedy = decodedGenrePreferences[4],
                Crime = decodedGenrePreferences[5],
                Documentary = decodedGenrePreferences[6],
                Drama = decodedGenrePreferences[7],
                Fantasy = decodedGenrePreferences[8],
                Horror = decodedGenrePreferences[9],
                Mystery = decodedGenrePreferences[10],
                Romance = decodedGenrePreferences[11],
                Scifi = decodedGenrePreferences[12],
                Thriller = decodedGenrePreferences[13]
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while decoding user preferences");
        }

        return preferences;
    }

}