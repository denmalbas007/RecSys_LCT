using RecSys.Api.Areas.Reports.Dtos.Graphs.Base;
using RecSys.Api.CommonDtos;

namespace RecSys.Api.Areas.Reports.Dtos;

public class RegionGrouping
{
    public Region Region { get; init; } = null!;

    public GraphBase[] Graphs { get; init; } = Array.Empty<GraphBase>();

    public AggregatedCustomsElement[] AggregatedCustomsElements { get; init; } =
        Array.Empty<AggregatedCustomsElement>();
}
