namespace RecSys.Api.CommonDtos;

public class Filter
{
    public long[] Regions { get; init; } = Array.Empty<long>();

    public long[] ItemTypes { get; init; } = Array.Empty<long>();

    public string[] Countries { get; init; } = Array.Empty<string>();
}
