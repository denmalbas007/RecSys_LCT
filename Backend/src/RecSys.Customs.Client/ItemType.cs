using System.Text.Json.Serialization;

namespace RecSys.Customs.Client;

public class ItemType
{
    [JsonPropertyName("hasChildNodes")]
    public bool HasChildNodes { get; init; }

    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;
}

public class ItemTypeV2
{
    public ItemTypeV2(ItemType itemType)
    {
        HasChildNodes = itemType.HasChildNodes;
        Id = itemType.Id;
        Name = itemType.Name;
    }

    [JsonPropertyName("hasChildNodes")]
    public bool HasChildNodes { get; init; }

    [JsonPropertyName("id")]
    public string Id { get; init; } = null!;

    [JsonPropertyName("label")]
    public string Name { get; init; } = null!;

    public static ItemTypeV2[] GetFromItemTypeArray(ItemType[] itemTypes)
        => itemTypes.Select(x => new ItemTypeV2(x)).ToArray();
}
