using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using LaPelicula.UI.Server.Models;
using UI.Shared;

namespace LaPelicula.UI.Server.Services;

public class ModelTrainingHostedService(
    ITensorFlowModelService _tensorFlowModelService, 
    IMovieRepository _movieRepository, 
    ILogger<ModelTrainingHostedService> _logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var rawMovieInfo = _tensorFlowModelService.Preprocess();
            foreach (var kvp in rawMovieInfo)
            {
                AddMovieToRepository(kvp);
            }
            _tensorFlowModelService.Train();
        });
        
    }
    
    private void AddMovieToRepository(KeyValuePair<long, IReadOnlyDictionary<string, PyObject>> kvp)
    {
        try
        {
            var (movieId, movieInfo) = (kvp.Key, kvp.Value);
            var movie = new Movie(
                movieId,
                movieInfo["tmdbId"].SafeAs<long>(),
                movieInfo["title"].SafeAs<string>(string.Empty) ,
                movieInfo["description"].SafeAs<string>(string.Empty) ,
                movieInfo["year"].IsNone() ? 0 : movieInfo["year"].As<long>(),
                movieInfo["poster_uri"].SafeAs<string>(string.Empty) ,
                movieInfo["genres"].SafeAs<IReadOnlyList<string>>([]).ToArray(),
                movieInfo["budget"].SafeAs<double>() ,
                movieInfo["origin_countries"].SafeAs<IReadOnlyList<string>>([]).ToArray());
            _movieRepository.AddMovie(movie);
        }
        catch (PythonInvocationException ex)
        {
            _logger.LogError(ex, "Error while trying to convert Python object for movie #{MovieID}", kvp.Key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding a movie to the repository #{MovieID}", kvp.Key);
        }
    }
}