using System.Globalization;
using System.IO.Compression;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Npgsql;
using NpgsqlTypes;
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
            var connection = (NpgsqlConnection)dbConnectionsProvider.GetConnection();
            await connection.OpenAsync(cancellationToken);
            do
            {
                var lastElement = await connection.QueryFirstOrDefaultAsync<CustomsElementRaw>(
                    query,
                    commandTimeout: 1000);
                DateTime startDate;
                startDate = lastElement is null ? new DateTime(2019, 01, 01) : lastElement.Period!.Value;
                if (startDate.Date >= DateTime.UtcNow.AddMonths(-1).Date)
                {
                    await Task.Delay(100000, cancellationToken);
                    break;
                }

                var stream = await customsClient.UnloadDataAsync(
                    startDate.AddMonths(1),
                    startDate.AddMonths(2),
                    cancellationToken);
                if (stream is null)
                {
                    await Task.Delay(100000, cancellationToken);
                    break;
                }

                using var archive = new ZipArchive(stream);
                var file = archive.Entries.FirstOrDefault();
                await using var csvStream = file?.Open();
                using var reader = new StreamReader(csvStream!);
                using var csv = new CsvReader(reader, config);
                var records = csv.GetRecords<CustomsElementRaw>();
                await using var writer =
                    await connection.BeginBinaryImportAsync(
                        "COPY customs (direction, period, country, item_type, unit_type, worth_total, gross_total, amount_total, region) FROM STDIN (FORMAT BINARY)",
                        cancellationToken);

                foreach (var record in records)
                {
                    if (!long.TryParse(record.Tnved, out var tnved)
                        || !long.TryParse(record.Region!.Split(" ")[0], out var reg))
                    {
                        continue;
                    }

                    try
                    {
                        await writer.StartRowAsync(cancellationToken);
                        await writer.WriteAsync(record.Napr == "ЭК", NpgsqlDbType.Boolean, cancellationToken);
                        await writer.WriteAsync(record.Period!.Value, NpgsqlDbType.Date, cancellationToken);
                        await writer.WriteAsync(record.Nastranapr, NpgsqlDbType.Text, cancellationToken);
                        await writer.WriteAsync(tnved, NpgsqlDbType.Bigint, cancellationToken);
                        if (record.Edizm.HasValue)
                            await writer.WriteAsync(record.Edizm.Value, NpgsqlDbType.Bigint, cancellationToken);
                        else
                            await writer.WriteNullAsync(cancellationToken);
                        await writer.WriteAsync(record.Stoim!.Value, NpgsqlDbType.Numeric, cancellationToken);
                        await writer.WriteAsync(record.Netto!.Value, NpgsqlDbType.Numeric, cancellationToken);
                        await writer.WriteAsync(record.Kol!.Value, NpgsqlDbType.Numeric, cancellationToken);
                        await writer.WriteAsync(reg, NpgsqlDbType.Bigint, cancellationToken);
                    }
                    catch
                    {
                        // ignore
                    }
                }

                await writer.CompleteAsync(cancellationToken);
            }
            while (true);

            await Task.Delay(50000, cancellationToken);
        }
    }
}
