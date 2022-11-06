namespace RecSys.Api.Areas.Reports.Dtos.Graphs.Base;

public abstract class GraphBase
{
    public long Id { get; init; }

    public abstract GraphDescriptor GraphDescriptor { get; init; }
}
