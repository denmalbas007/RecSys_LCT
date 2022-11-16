using System.Globalization;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using QuestPDF.Fluent;
using RecSys.Api.Areas.Reports.Dtos;
using RecSys.Api.Areas.Users.Dtos;
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
            try
            {
                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = "\t",
                };
                var dbConnectionsProvider = scope.ServiceProvider.GetRequiredService<IDbConnectionsProvider>();
                var client = scope.ServiceProvider.GetRequiredService<MlClient>();
                var taskQuery = "select * from reports where is_ready = false limit 1";
                var connection = dbConnectionsProvider.GetConnection();
                var report = await connection.QueryFirstOrDefaultAsync<ReportMetadata>(taskQuery);
                if (report is null)
                {
                    await Task.Delay(100, cancellationToken);
                    continue;
                }

                var regions11 = report.FilterOuter.Regions.Length > 0
                    ? await connection.QueryAsync<Region>(
                        "select * from regions where id = ANY(:Ids)",
                        new { Ids = report.FilterOuter.Regions })
                    : await connection.QueryAsync<Region>("select * from regions");

                foreach (var region in regions11)
                {
                    var sub = " ";
                    var sub2 = " ";
                    if (report.FilterOuter.Countries.Length > 0)
                        sub = " AND country = ANY(:Countries)";
                    if (report.FilterOuter.ItemTypes.Length > 0)
                        sub2 = " AND item_type = ANY(:ItemTypes)";
                    var periodsForRegion =
                        await connection.QueryAsync<DateTime>(
                            $@"select period from customs where region = :Region{sub}{sub2} group by period order by period desc limit 5",
                            new
                            {
                                report.FilterOuter.Countries,
                                report.FilterOuter.ItemTypes,
                                Region = region.Id
                            });
                    if (periodsForRegion.Count() < 2)
                        continue;

                    var dataForRegion = await connection.QueryAsync<CustomElementDb>(
                        $"select * from customs where region = :Region{sub}{sub2} AND period = ANY(:Periods)",
                        new
                        {
                            Periods = periodsForRegion.ToArray(),
                            report.FilterOuter.Countries,
                            report.FilterOuter.ItemTypes,
                            Region = region.Id
                        });

                    var records = dataForRegion.Select(
                        x => new CustomsElementRawMini
                        {
                            Napr = x.Direction ? "ЭК" : "ИМ",
                            Kol = x.AmountTotal,
                            Stoim = x.WorthTotal,
                            Period = x.Period,
                            Tnved = x.ItemType.ToString(),
                            Nastranapr = x.Country,
                            Netto = x.GrossTotal
                        }).ToList();
                    using var memoryStream = new MemoryStream();
                    await using var writer = new StreamWriter(memoryStream);
                    await using var csv = new CsvWriter(writer, config);
                    await csv.WriteRecordsAsync(records, cancellationToken);
                    await csv.FlushAsync();

                    // await csv.WriteRecordsAsync(records, cancellationToken);
                    var dict = await client.GetMlRangeAsync(
                        memoryStream.ToArray(),
                        periodsForRegion.ToArray(),
                        cancellationToken);
                    var insertQuery = @"insert into reports_data (report_id, region, item_type, coefficient)
VALUES (:ReportId, :Region, :ItemType, :Coefficient)";
                    await connection.ExecuteAsync(
                        insertQuery,
                        dict.Select(
                            x => new
                            {
                                ReportId = report.Id,
                                Region = region.Id,
                                ItemType = (long)float.Parse(x.Key),
                                Coefficient = x.Value
                            }));
                }

                // string pdfUrl;
                var getDataByReg =
                    @"select  rd.region, rd.item_type, it.name item_type_name, rd.coefficient coef from reports_data rd
inner join item_types it on rd.item_type = it.id where report_id = :Id";
                var elemtns = await connection.QueryAsync<CustomsElementForReport>(
                    getDataByReg,
                    new
                    {
                        report.Id
                    });
                var getRegions = await connection.QueryAsync<Region>(@"select * from regions");
                var getCountries = await connection.QueryAsync<Country>(@"select * from countries");
                var groupoing = elemtns.GroupBy(x => x.Region);
                var dicteq = new Dictionary<string, IEnumerable<CustomsElementForReport>>();
                var dicteq2 = new Dictionary<long, IDictionary<long, decimal>>();
                foreach (var gp in groupoing)
                {
                    var regq = getRegions.First(x => x.Id == gp.Key);
                    dicteq.Add(regq.Name, gp.ToArray());
                    dicteq2.Add(regq.Id, gp.DistinctBy(q => long.Parse(q.ItemType)).ToDictionary(x => long.Parse(x.ItemType), x => x.Coef));
                }

                var rowCount = 0;
                using var memor = new MemoryStream();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("stat");
                    rowCount = 1;
                    foreach (var line in dicteq)
                    {
                        worksheet.Cell(rowCount, 1).Value = line.Key;
                        rowCount++;
                        worksheet.Cell(rowCount, 1).Value = "ТК ВЭД";
                        worksheet.Cell(rowCount, 2).Value = "Обозначение";
                        worksheet.Cell(rowCount, 3).Value = "Коэффицент";
                        rowCount++;
                        foreach (var ww in line.Value)
                        {
                            worksheet.Cell(rowCount, 1).Value = ww.ItemType;
                            worksheet.Cell(rowCount, 2).Value = ww.ItemTypeName;
                            worksheet.Cell(rowCount, 3).Value = ww.Coef;
                            rowCount++;
                        }

                        rowCount++;
                    }

                    workbook.SaveAs(memor);
                }

                var qqqq = await client.GetGraphsAsync(dicteq2, cancellationToken);
                var query = @"select * from users inner join reports r on users.id = r.owner where r.id = :Id";
                var resultU = await connection.QueryFirstOrDefaultAsync<User>(query, new { report.Id });
                var doc = new ReportDocument(dicteq, report, resultU, getCountries, getRegions, qqqq);
                var bytes = doc.GeneratePdf();
                await File.WriteAllBytesAsync("test.pdf", bytes, cancellationToken);
                var baseString = Convert.ToBase64String(bytes);
                var baseString2 = Convert.ToBase64String(memor.ToArray());
                var guid = Guid.NewGuid();
                var guid2 = Guid.NewGuid();
                var pdfUrl = $"files/{guid}";
                var exUrl = $"files/{guid2}";
                var qqq = @"insert into storage (id, bytes, type) values (:Guid::uuid, :String, :Type)";
                await connection.ExecuteAsync(qqq, new { Guid = guid.ToString(), String = baseString, Type = "pdf" });
                await connection.ExecuteAsync(qqq, new { Guid = guid2.ToString(), String = baseString2, Type = "xlsx" });
                var updateQuery =
                    @"update reports set is_ready = true, pdf_url = :PdfUrl, excel_url = :ExcelUrl where id = :Id";
                await connection.ExecuteAsync(
                    updateQuery,
                    new
                    {
                        report.Id,
                        PdfUrl = pdfUrl,
                        ExcelUrl = exUrl
                    });
                await Task.Delay(100, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
