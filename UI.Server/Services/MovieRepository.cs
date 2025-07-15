using LaPelicula.UI.Server.Models;
using UI.Shared;

namespace LaPelicula.UI.Server.Services;

public interface IMovieRepository
{
    Task<Movie> GetMovieById(long movieId);
    Task AddMovie(Movie movie);
}

public class InMemoryMovieRepository : IMovieRepository
{
    private readonly Dictionary<long, Movie> _movies = new();
    public Task<Movie> GetMovieById(long movieId)
    {
        if (!_movies.ContainsKey(movieId))
        {
            throw new KeyNotFoundException("Movie with key {-#" + movieId + "#}  not found");
        }
        return Task.FromResult(_movies[movieId]);
    }

    public Task AddMovie(Movie movie)
    {
        _movies.Add(movie.Id, movie);
        return Task.CompletedTask;
    } 
}