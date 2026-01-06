using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using LaPelicula.UI.Server.Common;
using LaPelicula.UI.Shared;
using UI.Shared;

namespace LaPelicula.UI.Server.Services;

public interface ITensorFlowModelService
{
    ValueTask<IReadOnlyList<(long, double)>> RecommendAsync(UserPreferences userPreferences, IReadOnlyList<Movie> prefilteredMovies);
    TensorFlowModelStatus GetStatus();
    Task LoadAsync();
}

public class TensorFlowModelService : ITensorFlowModelService
{
    private readonly IPythonEnvironment _pythonEnvironment;
    private readonly ILogger<TensorFlowModelService> _logger;

    public TensorFlowModelService(IPythonEnvironment pythonEnvironment,  ILogger<TensorFlowModelService> logger)
    {
        _pythonEnvironment = pythonEnvironment;
        _logger = logger;
    }

    
    public async Task LoadAsync()
    {
        try
        {
            await Task.Run(() =>
            {
                _pythonEnvironment.RecommendationSystem()
                    .LoadModel();
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading the model: it might not have been trained yet");
            throw;
        }
    }
    
    private Int32[,] ConvertToMatrix(IReadOnlyList<Movie> movies)
    {
        var res = new Int32[movies.Count(), Genre.Count + 1];
        for (int i = 0; i < movies.Count(); i++)
        {
            var movie = movies[i];
            res[i, 0] = (int)movie.Id;
            for (int j = 0; j < movie.GenreVector.Length; j++)
            {
                res[i, j + 1] = movie.GenreVector[j];
            }
        }
        return res;
    }
    
    public async ValueTask<IReadOnlyList<(long, double)>> RecommendAsync(UserPreferences userPreferences, IReadOnlyList<Movie> prefilteredMovies)
    {
        try
        {
            var movieMatrix = ConvertToMatrix(prefilteredMovies);
            // ref int offset = ref allMovies.AsSpan2D().DangerousGetReference();
            byte[] buffer = new byte[movieMatrix.Length * sizeof(int)];
            unsafe
            {
                fixed (int* src = movieMatrix)
                fixed (byte* dst = buffer)
                {
                    Buffer.MemoryCopy(
                        src,
                        dst,
                        buffer.Length,
                        buffer.Length);
                }
            }
            return await Task.Run(() =>
            {
                
                return _pythonEnvironment.RecommendationSystem()
                    .Recommend(userPreferences.ToDictionary(), buffer);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while predicting recommendations");
            throw;
        }
    }
    
    public TensorFlowModelStatus GetStatus()
    {
        try
        {
            // local state is kept to avoid python GIL lock while training but we also would like to make sure that Python env is responsive at least, thus, calling is_trained()
            if (_pythonEnvironment.RecommendationSystem().IsLoaded()) 
                return TensorFlowModelStatus.Trained;
            else 
                return TensorFlowModelStatus.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while calling Python method");
            return TensorFlowModelStatus.Error;
        }
    }
}

internal static class PyObjectExtensions
{
    public static T SafeAs<T>(this PyObject obj, T defaultValue = default(T)) =>
        obj.IsNone() ? defaultValue : obj.As<T>();
}