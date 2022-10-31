namespace RecSys.Api.Areas.Reports.Dtos;

public sealed class Report : ReportMetadata
{
    public RegionGrouping[] Regions { get; init; } = Array.Empty<RegionGrouping>();

    public string PdfUrl { get; init; } = null!;

    public string ExcelUrl { get; init; } = null!;
}
