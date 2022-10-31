namespace RecSys.Api.Areas.Reports.Dtos;

public class ReportMetadata
{
    public long Id { get; init; }

    public string Name { get; init; } = null!;

    public DateTime CreatedAt { get; init; }
}
