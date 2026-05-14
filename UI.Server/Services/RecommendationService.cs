using LaPelicula.UI.Server.Common;
using LaPelicula.UI.Shared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UI.Shared;

namespace LaPelicula.UI.Server.Services;

public interface IRecommendationService
{
    Task<IEnumerable<Recommendation>> RecommendAsync(UserPreferences userPreferences, int limit = 25);
}

public class RecommendationService(
    IMovieRepository _movieRepository, 
    
    ITensorFlowModelService _tensorFlowModelService, 
    IMemoryCache _cache, 
    IOptions<RecommendationsConfig> _recommendationsConfig
) : IRecommendationService
{
    public async Task<IEnumerable<Recommendation>> RecommendAsync(UserPreferences userPreferences, int limit = 25)
    {
        var (prefilteredMovies, movieIdToMovie) = await GetPrefilteredMoviesAsync();
        var rawRecommendations = await _cache.GetOrCreateAsync(userPreferences.ToString()!, async entry =>
        {
            if (_recommendationsConfig.Value.CacheDurationSeconds > 0)
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(_recommendationsConfig.Value.CacheDurationSeconds)); // raw recommendations do not change
                
            return await _tensorFlowModelService.RecommendAsync(userPreferences, prefilteredMovies);
        });
        
        var result = new List<Recommendation>(capacity: rawRecommendations!.Count);
        foreach ((long movieId, double rating) in rawRecommendations)
        {
            var movie = movieIdToMovie[movieId];
            result.Add(new Recommendation(movie, Math.Floor(rating / 0.5) * 0.5));
        }

        var recommendationArr = result.ToArray();
        Random.Shared.Shuffle(recommendationArr);
        return recommendationArr;
    }


    
    
    private async Task<(List<Movie>, Dictionary<long, Movie>)> GetPrefilteredMoviesAsync()
    {
        // todo: add ANN-like search logic using annoy-like library
        var movies = (await _movieRepository.GetMoviesAsync()).ToList();
        return (movies, movies.ToDictionary(m => m.Id));
    }
}