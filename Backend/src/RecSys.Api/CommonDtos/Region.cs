namespace RecSys.Api.CommonDtos;

public class Region
{
    public District District { get; init; } = null!;

    public long Id { get; init; }

    public string Name { get; init; } = null!;
}
