using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using QuestPDF.Fluent;
using RecSys.Api.Areas.Reports.Dtos;
using RecSys.Api.CommonDtos;
using RecSys.Api.ReportTemplate;
using RecSys.ML.Client;
using RecSys.Platform.Data.Providers;

namespace RecSys.Api.Jobs;

public class BackgroundTasksProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BackgroundTasksProcessor(
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
            var dbConnectionsProvider = scope.ServiceProvider.GetRequiredService<IDbConnectionsProvider>();
            var client = scope.ServiceProvider.GetRequiredService<MlClient>();
            var taskQuery = "select * from reports where is_ready = false limit 1";
            var query = "select * from customs order by period DESC limit 1";
            var dataQuery = "select * from customs where period >= :From AND period <= :To";
            var connection = dbConnectionsProvider.GetConnection();
            var report = await connection.QueryFirstOrDefaultAsync<ReportMetadata>(taskQuery);
            if (report is null)
            {
                await Task.Delay(5000, cancellationToken);
                continue;
            }

            if (report.FilterOuter.Countries.Length > 0)
                dataQuery += " AND country = ANY(:Countries)";
            if (report.FilterOuter.Regions.Length > 0)
                dataQuery += " AND region = ANY(:Regions)";

            var custom = await connection.QueryFirstAsync<CustomElementDb>(query);
            var periods = new DateTime[]
            {
                custom.Period.AddMonths(-3),
                custom.Period.AddMonths(-2),
                custom.Period.AddMonths(-1),
                custom.Period
            };
            var data = await connection.QueryAsync<CustomElementDb>(
                dataQuery,
                new
                {
                    From = periods[0],
                    To = periods[3],
                    report.FilterOuter.Regions,
                    report.FilterOuter.Countries
                });
            var dataByReg = data.GroupBy(x => x.Region);
            var regions = dataByReg.Select(x => x.Key);
            foreach (var da in dataByReg)
            {
                var key = da.Key;
                var elements = da.ToArray();
                var recordsImport = elements
                    .Where(e => e.ImportAmountTotal != 0 || e.ImportNettoTotal != 0 || e.ImportWorthTotal != 0).Select(
                        x => new CustomsElementRawMini
                        {
                            Napr = "ИМ",
                            Kol = x.ImportAmountTotal,
                            Stoim = x.ImportWorthTotal,
                            Period = x.Period,
                            Tnved = x.ItemType,
                            Nastranapr = x.Country,
                            Netto = x.ImportNettoTotal
                        });
                var recordsExport = elements
                    .Where(e => e.ExportAmountTotal != 0 || e.ExportNettoTotal != 0 || e.ExportWorthTotal != 0)
                    .Select(
                        x => new CustomsElementRawMini
                        {
                            Napr = "ЭК",
                            Kol = x.ExportAmountTotal,
                            Stoim = x.ExportWorthTotal,
                            Period = x.Period,
                            Tnved = x.ItemType,
                            Nastranapr = x.Country,
                            Netto = x.ExportNettoTotal
                        });
                using var memoryStream = new MemoryStream();
                await using var writer = new StreamWriter(memoryStream);
                await using var csv = new CsvWriter(writer, config);
                await csv.WriteRecordsAsync(recordsExport, cancellationToken);
                await csv.WriteRecordsAsync(recordsImport, cancellationToken);
                var dict = await client.GetMlRangeAsync(memoryStream.ToArray(), periods, cancellationToken);
                // .WithColumn("report_id").AsInt64().ForeignKey("reports", "id")
                //     .WithColumn("region").AsInt64().ForeignKey("regions", "id")
                //     .WithColumn("item_type").AsString().ForeignKey("item_types", "id")
                //     .WithColumn("coefficient").AsFloat();
                var insertQuery = @"insert into reports_data (report_id, region, item_type, coefficient)
VALUES (:ReportId, :Region, :ItemType, :Coefficient)";
                await connection.ExecuteAsync(
                    insertQuery,
                    dict.Select(
                        x => new
                        {
                            ReportId = report.Id,
                            Region = key,
                            ItemType = x.Key,
                            Coefficient = x.Value
                        }));
            }

            // string pdfUrl;
            var getDataByReg =
                @"select  rd.region, rd.item_type, it.name item_type_name, rd.coefficient coef from reports_data rd
inner join item_types it on rd.item_type = it.id where report_id = :Id";
            var elemtns = await connection.QueryAsync<CustomsElementForReport>(getDataByReg, new
            {
                report.Id
            });
            var getRegions = await connection.QueryAsync<Region>(@"select * from regions");
            var groupoing = elemtns.GroupBy(x => x.Region);
            var dicteq = new Dictionary<string, IEnumerable<CustomsElementForReport>>();
            foreach (var gp in groupoing)
            {
                var regq = getRegions.First(x => x.Id == gp.Key);
                dicteq.Add(regq.Name, gp.ToArray());
            }

            var doc = new ReportDocument(dicteq);
            var bytes = doc.GeneratePdf();
            var baseString = Convert.ToBase64String(bytes);
            var guid = Guid.NewGuid();
            var pdfUrl = $"files/{guid};";
            var qqq = @"insert into storage (id, bytes, type) values (:Guid::uuid, :String, :Type)";
            await connection.ExecuteAsync(qqq, new { Guid = guid.ToString(), String = baseString, Type = "pdf" });
            var updateQuery = @"update reports set is_ready = true, pdf_url = :PdfUrl, excel_url = :ExcelUrl where id = :Id";
            await connection.ExecuteAsync(updateQuery, new
            {
                report.Id,
                PdfUrl = pdfUrl,
                ExcelUrl = pdfUrl
            });
            await Task.Delay(1000, cancellationToken);
        }
    }
}
