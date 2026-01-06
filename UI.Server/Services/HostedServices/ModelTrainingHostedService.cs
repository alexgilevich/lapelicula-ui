using CSnakes.Runtime;
using CSnakes.Runtime.Python;
using UI.Shared;

namespace LaPelicula.UI.Server.Services.HostedServices;

public class ModelTrainingHostedService(
    ITensorFlowModelService _tensorFlowModelService, 
    IMovieRepository _movieRepository, 
    ILogger<ModelTrainingHostedService> _logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            await _tensorFlowModelService.LoadAsync();
        });
        
    }
}