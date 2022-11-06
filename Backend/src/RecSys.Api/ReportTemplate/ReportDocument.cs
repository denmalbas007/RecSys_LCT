using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RecSys.Api.CommonDtos;

namespace RecSys.Api.ReportTemplate;

public class ReportDocument : IDocument
{
    private readonly IDictionary<string, IEnumerable<CustomsElementForReport>> _dictionary;

    public ReportDocument(IDictionary<string, IEnumerable<CustomsElementForReport>> dictionary)
        => _dictionary = dictionary;

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
        => container.Page(
            page =>
            {
                page.MarginHorizontal(10f);
                page.MarginVertical(10f);
                page.DefaultTextStyle(x => x.FontFamily(Fonts.TimesNewRoman));
                page.Size(PageSizes.A4.Landscape());
                page.PageColor(Colors.White);
                page
                    .Content()
                    .Column(
                        x =>
                        {
                            foreach (var (key, val) in _dictionary)
                            {
                                x.Item().Column(
                                    conx =>
                                    {
                                        conx.Item().Text(key);
                                        conx.Item().Table(
                                            x =>
                                            {
                                                x.ColumnsDefinition(
                                                    q
                                                        =>
                                                    {
                                                        q.RelativeColumn();
                                                        q.RelativeColumn();
                                                        q.RelativeColumn();
                                                    });
                                                x.Header(
                                                    y =>
                                                    {
                                                        y.Cell().Border(1f).Text("ТН ВЕД").FontSize(8f);
                                                        y.Cell().Border(1f).Text("Обозначение").FontSize(8f);
                                                        y.Cell().Border(1f).Text("Коэффицент импортозамещения").FontSize(8f);
                                                    });
                                                foreach (var el in val)
                                                {
                                                    x.Cell().Border(1).Text(el.ItemType).FontSize(8f);
                                                    x.Cell().Border(1).Text(el.ItemTypeName).FontSize(8f);
                                                    x.Cell().Border(1).Text(el.Coef).FontSize(8f);
                                                }
                                            });
                                    });
                            }
                        });
            });
}
