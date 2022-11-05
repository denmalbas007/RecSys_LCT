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
