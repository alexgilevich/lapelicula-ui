using UI.Shared;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace LaPelicula.UI.Server.Services;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetTopMoviesAsync(int limit = 200);
}

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private IMemoryCache _cache;

    public MovieService(IMovieRepository movieRepository, IMemoryCache cache)
    {
        _cache = cache;
        _movieRepository = movieRepository;
    }
    
    public async Task<IEnumerable<Movie>> GetTopMoviesAsync(int limit = 200)
    {
        // using double cache layer is justifiable here because it avoids unnecessary CPU load on the server
        // accuracy of the top movies is not critical so we can afford to cache it for a while
        return (await _cache.GetOrCreateAsync($"movies_top{limit}", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(180));
            var movies = await _movieRepository.GetMoviesAsync();
            return movies.OrderBy(m => m.RatingAverage).Take(limit).ToList();
        }))!; // GetOrCreateAsync guarantees that the result is not null
    }
    
}