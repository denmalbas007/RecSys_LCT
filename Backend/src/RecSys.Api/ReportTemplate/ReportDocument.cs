using System.Globalization;
using QRCoder;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RecSys.Api.Areas.Reports.Dtos;
using RecSys.Api.Areas.Users.Dtos;
using RecSys.Api.CommonDtos;

namespace RecSys.Api.ReportTemplate;

public class ReportDocument : IDocument
{
    private readonly IDictionary<string, IEnumerable<CustomsElementForReport>> _dictionary;
    private readonly string _reportName;
    private readonly long _reportId;
    private readonly User _user;
    private readonly ReportMetadata _report;
    private readonly IEnumerable<Region> _regions;
    private readonly IEnumerable<Country> _countries;
    private readonly IDictionary<long, byte[]> _graphs;

    public ReportDocument(IDictionary<string, IEnumerable<CustomsElementForReport>> dictionary, ReportMetadata report, User user, IEnumerable<Country> countries, IEnumerable<Region> regions, IDictionary<long, byte[]> graphs)
    {
        _dictionary = dictionary;
        _reportName = report.Name;
        _reportId = report.Id;
        _report = report;
        _user = user;
        _countries = countries;
        _regions = regions;
        _graphs = graphs;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
        => container.Page(
            page =>
            {
                page.MarginHorizontal(10f);
                page.MarginVertical(10f);
                page.DefaultTextStyle(x => x.FontFamily(Fonts.Arial));
                page.Size(PageSizes.A4.Landscape());
                page.PageColor(Colors.White);
                page
                    .Content()
                    .Column(
                        x =>
                        {
                            x.Item().Row(r =>
                            {
                                r.RelativeItem(100).AlignLeft().MaxWidth(300).Image("ReportTemplate/Resources/logo.png");
                                r.RelativeItem(100).AlignRight().AlignMiddle().MaxWidth(70).Image(
                                    GetQr($"%RESCXXR%{_reportId}%{_reportName}"));
                            });
                            x.Item().Text(
                                te =>
                                {
                                    te.Span("Отчет").Bold().FontSize(18f).FontColor(Colors.Grey.Darken2);
                                    te.Span(" о вычисления перспективных производственных ниш по регионам от ")
                                        .FontSize(18f).FontColor(Colors.Grey.Darken2);
                                    te.Span($"{_report.CreatedAt.Date:dd.MM.yyyy}").FontSize(18f).FontColor(Colors.Grey.Darken2).Italic();
                                });
                            x.Item().Container().Height(10);
                            x.Item().LineHorizontal(1f).LineColor(Colors.Grey.Darken1);
                            x.Item().Container().Height(10);

                            x.Item().Text(
                                te =>
                                {
                                    te.Span("Наименование: ").FontSize(10f).FontColor(Colors.Grey.Darken2);
                                    te.Span($"{_report.Name}")
                                        .FontSize(10f).FontColor(Colors.Grey.Darken2).Italic();
                                });

                            x.Item().Text(
                                te =>
                                {
                                    te.Span("Дата создания: ").FontSize(10f).FontColor(Colors.Grey.Darken2);
                                    te.Span($"{_report.CreatedAt.Date:dd.MM.yyyy}")
                                        .FontSize(10f).FontColor(Colors.Grey.Darken2).Italic();
                                });

                            x.Item().Text(
                                te =>
                                {
                                    te.Span("Оператор: ").FontSize(10f).FontColor(Colors.Grey.Darken2);
                                    te.Span($"{_user.SecondName} {_user.FirstName} ({_user.Email})")
                                        .FontSize(10f).FontColor(Colors.Grey.Darken2).Italic();
                                });
                            x.Item().Text(
                                te =>
                                {
                                    te.Span("Страны: ").FontSize(10f).FontColor(Colors.Grey.Darken2);
                                    te.Span(_report.FilterOuter.Countries.Length <= 0 ? "ВСЕ" : string.Join(",", _countries.Where(c => _report.FilterOuter.Countries.Contains(c.Id))
                                            .Select(c => c.Name)))
                                    .FontSize(10f).FontColor(Colors.Grey.Darken2).Italic();
                                });
                            x.Item().Text(
                                te =>
                                {
                                    te.Span("Регионы: ").FontSize(10f).FontColor(Colors.Grey.Darken2);
                                    te.Span(_report.FilterOuter.Regions.Length <= 0 ? "ВСЕ" : string.Join(",", _regions
                                            .Where(c => _report.FilterOuter.Regions.Contains(c.Id))
                                            .Select(c => c.Name)))
                                        .FontSize(10f).FontColor(Colors.Grey.Darken2).Italic();
                                });
                            x.Item().Text(
                                te =>
                                {
                                    te.Span("ТК ВЭД: ").FontSize(10f).FontColor(Colors.Grey.Darken2);
                                    te.Span(_report.FilterOuter.ItemTypes.Length <= 0 ? "ВСЕ" : string.Join(",", _report.FilterOuter.ItemTypes))
                                        .FontSize(10f).FontColor(Colors.Grey.Darken2).Italic();
                                });
                            x.Item().Container().Height(10);
                            x.Item().LineHorizontal(1f).LineColor(Colors.Grey.Darken1);
                            x.Item().Container().Height(10);
                            var counteqq = 0;
                            foreach (var (key, val) in _dictionary)
                            {
                                x.Item().ShowIf(counteqq > 0).PageBreak();
                                counteqq++;
                                x.Item().Column(
                                    conx =>
                                    {
                                        conx.Item().Text(
                                            tex =>
                                            {
                                                tex.Span("Регион: ").Bold().FontSize(16f)
                                                    .FontColor(Colors.Grey.Darken2);
                                                tex.Span(key).Italic();
                                            });
                                        conx.Item().Image(_graphs[_regions.First(x => x.Name == key).Id]);
                                        conx.Item().Table(
                                            x =>
                                            {
                                                x.ColumnsDefinition(
                                                    q
                                                        =>
                                                    {
                                                        q.RelativeColumn(8);
                                                        q.RelativeColumn(72);
                                                        q.RelativeColumn(20);
                                                    });
                                                x.Header(
                                                    y =>
                                                    {
                                                        y.Cell().Border(1f).AlignCenter().AlignMiddle().Padding(2)
                                                            .Text(
                                                                ee =>
                                                                {
                                                                    ee.Span("ТН ВЭД").FontSize(10f).Bold().Italic();
                                                                    ee.Span(" 1").FontSize(10f).Superscript().Italic()
                                                                        .FontColor(Colors.Grey.Darken2);
                                                                });
                                                        y.Cell().Border(1f).AlignCenter().AlignMiddle().Padding(2)
                                                            .Text(
                                                                ee =>
                                                                {
                                                                    ee.Span("Расшифровка ТН ВЭД").FontSize(10f).Bold()
                                                                        .Italic();
                                                                    ee.Span(" 1").FontSize(10f).Superscript().Italic()
                                                                        .FontColor(Colors.Grey.Darken2);
                                                                });
                                                        y.Cell().Border(1f).AlignCenter().AlignMiddle().Padding(2)
                                                            .Text(
                                                                ee =>
                                                                {
                                                                    ee.Span("Коэффициент").FontSize(10f).Bold()
                                                                        .Italic();
                                                                    ee.Span(" 2").FontSize(10f).Superscript().Italic()
                                                                        .FontColor(Colors.Grey.Darken2);
                                                                });
                                                    });
                                                var count = 0;
                                                var valL = val.ToList();
                                                foreach (var el in valL)
                                                {
                                                    var color = "#f2978d";
                                                    if (count < (valL.Count * 10 / 100))
                                                    {
                                                        color = "#adff96";
                                                    }
                                                    else if (count < (valL.Count * 90 / 100))
                                                    {
                                                        color = "#f5ff69";
                                                    }

                                                    x.Cell().Background(color).Border(1).AlignCenter().AlignMiddle()
                                                        .Padding(2)
                                                        .Text(el.ItemType).FontSize(8f);
                                                    x.Cell().Background(color).Border(1).AlignCenter().AlignLeft()
                                                        .Padding(2)
                                                        .Text(el.ItemTypeName).FontSize(8f);
                                                    x.Cell().Background(color).Border(1).AlignCenter().AlignMiddle()
                                                        .Padding(2).Text(
                                                            Math.Round(el.Coef, 3)
                                                                .ToString(CultureInfo.InvariantCulture))
                                                        .FontSize(8f);
                                                    count++;
                                                }
                                            });
                                    });
                            }

                            x.Item().Container().Height(10);
                            x.Item().LineHorizontal(1f).LineColor(Colors.Grey.Darken1);
                            x.Item().Container().Height(10);
                            x.Item().Text(
                                "1 - Товарная номенклатура внешнеэкономической деятельности Евразийского экономического союза")
                                .FontColor(Colors.Grey.Lighten1).Italic().FontSize(6f);
                            x.Item().Text(
                                    "2 - Коэффицент перспективности импортозамещения")
                                .FontColor(Colors.Grey.Lighten1).Italic().FontSize(6f);
                        });
            });

    private byte[] GetQr(string value)
    {
        var data = QRCodeGenerator.GenerateQrCode(value, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(data);
        return qrCode.GetGraphic(10);
    }
}
