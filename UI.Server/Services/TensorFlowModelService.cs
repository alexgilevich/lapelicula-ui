using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using LaPelicula.UI.Server.Models;
using LaPelicula.UI.Shared;

namespace LaPelicula.UI.Server.Services;

public interface ITensorFlowModelService
{
    void Train();
    IReadOnlyList<Recommendation> Recommend(UserPreferences userPreferences);
    TensorFlowModelStatus GetStatus();
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

    public void Train()
    {
        try
        {
            // keep local state to avoid python GIL lock
            _status = TensorFlowModelStatus.InProgress;
            _pythonEnvironment.RecommendationSystem().Preprocess("./ml/data");
            _pythonEnvironment.RecommendationSystem().Train("./ml/artifacts/model.keras");
        }
        catch (Exception ex)
        {
            _status = TensorFlowModelStatus.Error;
            throw;
        }
        
    }
    
    public IReadOnlyList<Recommendation> Recommend(UserPreferences userPreferences)
    {
        try
        {
            return _pythonEnvironment.RecommendationSystem()
                .Recommend([], userPreferences.ToDictionary())
                .Select(
                    ((string movieName, double rating) x) => new Recommendation(x.movieName, x.rating))
                .ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while predicting recommendations");
            return [];
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