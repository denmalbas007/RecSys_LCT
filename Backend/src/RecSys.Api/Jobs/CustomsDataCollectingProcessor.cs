using RecSys.Api.CommonDtos;
using RecSys.Api.DataAccess.Customs;
using RecSys.Customs.Client;

namespace RecSys.Api.Jobs;

public class CustomsDataCollectingProcessor
{
    private readonly CustomsClient _customsClient;
    private readonly CustomsRepository _customsRepository;

    public CustomsDataCollectingProcessor(CustomsClient customsClient, CustomsRepository customsRepository)
    {
        _customsClient = customsClient;
        _customsRepository = customsRepository;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        while (cancellationToken.IsCancellationRequested)
        {
            try
            {
                var (customsElement, _) = await _customsRepository.GetCustomsElementsByFilter(
                    new Filter(),
                    new Pagination(0, 1),
                    cancellationToken);
                var element = customsElement.First();
                if (element.Period < DateTime.Now.AddMonths(-1))
                {
                    var stream = await _customsClient.UnloadDataAsync(
                        element.Period,
                        element.Period.AddMonths(1),
                        cancellationToken);
                }
            }
            catch
            {
                // ignored
            }

            await Task.Delay(50000, cancellationToken);
        }
    }
}
