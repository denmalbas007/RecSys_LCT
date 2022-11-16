using RecSys.Api.Jobs;

namespace RecSys.Api.Infrastructure;

public class MainHostedService : BackgroundService
{
    private readonly CustomsDataCollectingProcessor _dataCollectingProcessor;
    // private readonly DataProcessingProcessor _dataProcessingProcessor;
    private readonly BackgroundTasksProcessor _backgroundTasksProcessor;

    public MainHostedService(
        CustomsDataCollectingProcessor dataCollectingProcessor,
        // DataProcessingProcessor dataProcessingProcessor,
        BackgroundTasksProcessor backgroundTasksProcessor)
    {
        _dataCollectingProcessor = dataCollectingProcessor;
        _backgroundTasksProcessor = backgroundTasksProcessor;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            return Task.WhenAny(
                _dataCollectingProcessor.StartAsync(stoppingToken),
                _backgroundTasksProcessor.StartAsync(stoppingToken));
        }
        catch
        {
            return Task.CompletedTask;
        }
    }
}
