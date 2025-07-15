using LaPelicula.UI.Server.Models;
using LaPelicula.UI.Shared;
using Microsoft.Extensions.Caching.Memory;
using UI.Shared;

namespace LaPelicula.UI.Server.Services;

public interface IRecommendationService
{
    Task<IEnumerable<Recommendation>> RecommendAsync(string encodedPreferencesStr, int limit = 25);
}

public class RecommendationService(IMovieRepository _movieRepository, IUserPreferencesEncoder _userPreferencesEncoder, ITensorFlowModelService _tensorFlowModelService, IMemoryCache _cache) : IRecommendationService
{
    public async Task<IEnumerable<Recommendation>> RecommendAsync(string encodedPreferencesStr, int limit = 25)
    {
        var preferences = _userPreferencesEncoder.Decode(encodedPreferencesStr);
        var rawRecommendations = await _cache.GetOrCreateAsync(encodedPreferencesStr, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(60)); // raw recommendations do not change
            return await _tensorFlowModelService.RecommendAsync(preferences);
            
        });
        
        var result = new List<Recommendation>(capacity: rawRecommendations!.Count);
        foreach ((long movieId, double rating) in rawRecommendations)
        {
            var movie = await _movieRepository.GetMovieById(movieId);
            result.Add(new Recommendation(movie, rating));
        }

        var recommendationArr = result.ToArray();
        Random.Shared.Shuffle(recommendationArr);
        return recommendationArr;
    }
}