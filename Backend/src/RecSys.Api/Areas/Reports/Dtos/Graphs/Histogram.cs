using RecSys.Api.Areas.Reports.Dtos.Graphs.Base;

namespace RecSys.Api.Areas.Reports.Dtos.Graphs;

public class Histogram : GraphBase
{
    public override GraphDescriptor GraphDescriptor { get; init; } = GraphDescriptor.Histogram;
}
