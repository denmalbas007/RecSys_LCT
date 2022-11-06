namespace RecSys.Api.Areas.Reports.Dtos;

public sealed class Report : ReportMetadata
{
    public RegionGrouping[] Regions { get; init; } = Array.Empty<RegionGrouping>();
}
