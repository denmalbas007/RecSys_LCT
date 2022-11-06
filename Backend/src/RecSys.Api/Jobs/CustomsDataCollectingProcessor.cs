using System.Globalization;
using System.IO.Compression;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using RecSys.Api.CommonDtos;
using RecSys.Customs.Client;
using RecSys.Platform.Data.Providers;

namespace RecSys.Api.Jobs;

public class CustomsDataCollectingProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CustomsDataCollectingProcessor(
        IServiceScopeFactory serviceScopeFactory)
        => _serviceScopeFactory = serviceScopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
            };
            var customsClient = scope.ServiceProvider.GetRequiredService<CustomsClient>();
            var dbConnectionsProvider = scope.ServiceProvider.GetRequiredService<IDbConnectionsProvider>();
            var query = "select * from customs order by period DESC limit 1";
            var insertQuery =
                @"insert into customs_raw (napr, period, nastranapr, tnved, edizm, stoim, netto, kol, region, region_s)
                     values (:Napr, :Period, :Nastranapr, :Tnved, :Edizm, round(:Stoim, 5), round(:Netto, 5), round(:Kol, 5), :Region, :RegionsS)";
            var connection = dbConnectionsProvider.GetConnection();
            do
            {
                var lastElement = await connection.QueryFirstOrDefaultAsync<CustomsElementRaw>(
                    query, commandTimeout: 1000);
                DateTime startDate;
                startDate = lastElement is null ? new DateTime(2021, 08, 01) : lastElement.Period!.Value;
                if (startDate > DateTime.UtcNow.AddMonths(-1))
                    break;
                var stream = await customsClient.UnloadDataAsync(
                    startDate,
                    startDate.AddMonths(1),
                    cancellationToken);
                if (stream is null)
                    break;
                using var archive = new ZipArchive(stream);
                var file = archive.Entries.FirstOrDefault();
                await using var csvStream = file?.Open();
                using var reader = new StreamReader(csvStream!);
                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<CustomsElementRaw>();
                var chunks = records.Chunk(100).Chunk(10);
                var tasks = new List<Task>();
                {
                    foreach (var chunk in chunks)
                    {
                        foreach (var ch in chunk)
                        {
                            tasks.Add(ExecuteChunkAsync(ch, insertQuery));
                        }

                        await Task.WhenAll(tasks);
                    }
                }
            }
            while (true);

            await Task.Delay(50000, cancellationToken);
        }
    }

    private async Task ExecuteChunkAsync(CustomsElementRaw[] chunk, string insertQuery)
    {
        try
        {
            await using var innerScope = _serviceScopeFactory.CreateAsyncScope();
            var connPr = innerScope.ServiceProvider.GetRequiredService<IDbConnectionsProvider>();
            var conn = connPr.GetConnection();
            await conn.ExecuteAsync(insertQuery, chunk, commandTimeout: 1000);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            foreach (var a in chunk)
            {
                Console.WriteLine(a.Kol + " " + a.Netto + " " + a.Stoim);
            }
        }
    }
}
