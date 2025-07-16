using System.Text.Json;
using UI.Shared;

namespace LaPelicula.UI.Client.Services;

public class RecommendationsHttpService(HttpClient httpClient) : IRecommendationsHttpService
{
    public async Task<List<Recommendation>> GetRecommendations()
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = await httpClient.GetAsync("api/recommendations");
        var content = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
        {
            throw new ModelNotReadyException();
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            throw new CookieNotSetException();
        }
        else if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }
        
        return JsonSerializer.Deserialize<List<Recommendation>>(content, options) ?? throw new InvalidOperationException();
    }
}


public class ModelNotReadyException() : Exception("Model is not ready yet");

public class CookieNotSetException() : Exception("Define preferences first");