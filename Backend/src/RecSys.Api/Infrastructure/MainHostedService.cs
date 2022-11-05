using RecSys.Api.Jobs;

namespace RecSys.Api.Infrastructure;

public class MainHostedService : BackgroundService
{
    private readonly CustomsDataCollectingProcessor _dataCollectingProcessor;
    private readonly DataProcessingProcessor _dataProcessingProcessor;

    public MainHostedService(
        CustomsDataCollectingProcessor dataCollectingProcessor,
        DataProcessingProcessor dataProcessingProcessor)
    {
        _dataCollectingProcessor = dataCollectingProcessor;
        _dataProcessingProcessor = dataProcessingProcessor;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            return Task.WhenAll(
                _dataCollectingProcessor.StartAsync(stoppingToken),
                _dataProcessingProcessor.StartAsync(stoppingToken));
        }
        catch
        {
            return Task.CompletedTask;
        }
    }
}
