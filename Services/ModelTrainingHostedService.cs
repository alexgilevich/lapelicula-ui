namespace LaPelicula.UI.Services;

public class ModelTrainingHostedService(ITensorFlowModelService tensorFlowModelService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            tensorFlowModelService.Train();
        });
        
    }
}