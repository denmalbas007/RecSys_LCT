using Dapper;
using RecSys.Api.CommonDtos;
using RecSys.Platform.Data.Providers;

namespace RecSys.Api.Jobs;

public class DataProcessingProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DataProcessingProcessor(IServiceScopeFactory serviceScopeFactory)
        => _serviceScopeFactory = serviceScopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var dbConnectionsProvider = scope.ServiceProvider.GetRequiredService<IDbConnectionsProvider>();
            var dbTransactionProvider = scope.ServiceProvider.GetRequiredService<IDbTransactionsProvider>();
            await using var transaction = await dbTransactionProvider.BeginTransactionAsync(cancellationToken);
            var query = "select * from customs_raw where is_processed = false limit 10000";
            var connection = dbConnectionsProvider.GetConnection();
            var unprocessedElements = await connection.QueryAsync<CustomsElementRaw>(
                query,
                transaction: dbTransactionProvider.Current,
                commandTimeout: 1000);
            var elementsToProcess = unprocessedElements.Select(
                x => new CustomElementDb
                {
                    Country = x.Nastranapr!,
                    ExportAmountTotal = x.Napr! == "ИМ" ? 0 : x.Kol ?? 0,
                    ExportNettoTotal = x.Napr! == "ИМ" ? 0 : x.Netto ?? 0,
                    ExportWorthTotal = x.Napr! == "ИМ" ? 0 : x.Stoim ?? 0,
                    ImportAmountTotal = x.Napr! == "ЭК" ? 0 : x.Kol ?? 0,
                    ImportNettoTotal = x.Napr! == "ЭК" ? 0 : x.Netto ?? 0,
                    ImportWorthTotal = x.Napr! == "ЭК" ? 0 : x.Stoim ?? 0,
                    UnitType = x.Edizm,
                    Period = x.Period!.Value,
                    ItemType = x.Tnved!,
                    Region = long.Parse(x.Region!.Split("-").First().TrimEnd())
                });
            var insertQuery =
                @"insert into customs (item_type, period, country, region, unit_type, import_worth_total, import_netto_total, import_amount_total, export_worth_total, export_netto_total, export_amount_total)
                                values (:ItemType, :Period, :Country, :Region, :UnitType, :ImportWorthTotal, :ImportNettoTotal, :ImportAmountTotal, :ExportWorthTotal, :ExportNettoTotal, :ExportAmountTotal)
                                on conflict (item_type, period, country, region, unit_type)
                                do update set import_amount_total = customs.import_amount_total + excluded.import_amount_total,
                                              import_netto_total = customs.import_netto_total + excluded.import_netto_total,
                                              import_worth_total = customs.import_worth_total + excluded.import_worth_total,
                                              export_amount_total = customs.export_amount_total + excluded.export_amount_total,
                                              export_netto_total = customs.export_netto_total + excluded.export_netto_total,
                                              export_worth_total = customs.export_worth_total + excluded.export_worth_total";
            await connection.ExecuteAsync(insertQuery, elementsToProcess, dbTransactionProvider.Current);
            var markAsDoneQuery = @"update customs_raw set is_processed = true where id = ANY(:Ids)";
            await connection.ExecuteAsync(
                markAsDoneQuery,
                new { Ids = unprocessedElements.Select(x => x.Id).ToArray() },
                transaction: transaction,
                commandTimeout: 1000);
            await transaction.CommitAsync(cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }
    }
}
