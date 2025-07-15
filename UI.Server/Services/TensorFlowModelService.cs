using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using LaPelicula.UI.Server.Models;
using LaPelicula.UI.Shared;

namespace LaPelicula.UI.Server.Services;

public interface ITensorFlowModelService
{
    void Train();
    ValueTask<IReadOnlyList<(long, double)>> RecommendAsync(UserPreferences userPreferences);
    TensorFlowModelStatus GetStatus();
    IReadOnlyDictionary<long, IReadOnlyDictionary<string, PyObject>> Preprocess();
}

public class TensorFlowModelService : ITensorFlowModelService
{
    private readonly IPythonEnvironment _pythonEnvironment;
    private readonly ILogger<TensorFlowModelService> _logger;
    private TensorFlowModelStatus _status;
    
    public TensorFlowModelService(IPythonEnvironment pythonEnvironment, ILogger<TensorFlowModelService> logger)
    {
        _pythonEnvironment = pythonEnvironment;
        _logger = logger;
    }

    public IReadOnlyDictionary<long, IReadOnlyDictionary<string, PyObject>> Preprocess()
    {
        try
        {
            // keep local state to avoid python GIL lock
            _status = TensorFlowModelStatus.InProgress;
            return _pythonEnvironment.RecommendationSystem().Preprocess("./ml/data");
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while preprocessing data");
            _status = TensorFlowModelStatus.Error;
            throw;
        }
    }
    
    public void Train()
    {
        try
        {
            // keep local state to avoid python GIL lock
            _status = TensorFlowModelStatus.InProgress;
            _pythonEnvironment.RecommendationSystem().Train("./ml/artifacts/model.keras");
            _status = TensorFlowModelStatus.Trained;
        }
        catch (PythonInvocationException exc)
        {
            _logger.LogError(exc, "Error occurred while training TensorFlow model");
            _status = TensorFlowModelStatus.Error;
            throw;
        }

    }

    public async ValueTask<IReadOnlyList<(long, double)>> RecommendAsync(UserPreferences userPreferences)
    {
        try
        {
            return await Task.Run(() =>
            {
                return _pythonEnvironment.RecommendationSystem()
                    .Recommend([], userPreferences.ToDictionary());
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
            return _status switch
            {
                TensorFlowModelStatus.Trained when _pythonEnvironment.RecommendationSystem().IsTrained() =>
                    TensorFlowModelStatus.Trained,
                _ => _status
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting the status of the model");
            return TensorFlowModelStatus.Error;
        }
    }
}

internal static class PyObjectExtensions
{
    public static T SafeAs<T>(this PyObject obj, T defaultValue = default(T)) =>
        obj.IsNone() ? defaultValue : obj.As<T>();
}