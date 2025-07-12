using System.Text.Json;

namespace LaPelicula.UI.Client.Services;

public interface IRecommendationsHttpService
{
    Task<List<string>> GetRecommendations();
}

public class RecommendationsHttpService(HttpClient httpClient) : IRecommendationsHttpService
{
    public async Task<List<string>> GetRecommendations()
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var response = await httpClient.GetAsync("api/recommendations");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }
        return JsonSerializer.Deserialize<List<string>>(content, options) ?? throw new InvalidOperationException();
    }
}