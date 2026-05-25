using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using LaPelicula.UI.Server.Services;
using UI.Shared;

namespace LaPelicula.UI.Client.Services;

/// <summary>
/// Stub class returning random movies the top 200 static list for SEO purposes mostly
/// </summary>
public class PrerenderedRecommendationsHttpService : IRecommendationsHttpService
{
    private IMovieService _movieService;

    public PrerenderedRecommendationsHttpService(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<List<Recommendation>> GetRecommendationsAsync()
    {
        var topMovies = (await _movieService.GetTopMoviesAsync()).ToArray();
        Random.Shared.Shuffle(topMovies);
        
        var recommendations = topMovies.Take(25).Select(m => new Recommendation(m, m.RatingAverage)).ToList();
        
        return recommendations;
    }
}

