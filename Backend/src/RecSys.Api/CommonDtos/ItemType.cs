namespace RecSys.Api.CommonDtos;

public class ItemType
{
    public ItemType? Child { get; init; }

    public long Id { get; init; }

    public string Name { get; init; } = null!;
}
