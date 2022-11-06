using System.Text.Json.Serialization;
using RecSys.Api.CommonDtos;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RecSys.Api.Areas.Layouts.Dtos;

public class Layout
{
    public long Id { get; init; }

    public string Name { get; init; } = null!;

    [JsonPropertyName("filter")]
    public Filter FilterOuter { get; init; } = null!;

    [JsonIgnore]
    public string Filter
    {
        init => FilterOuter = JsonSerializer.Deserialize<Filter>(value)!;
    }

    public DateTime UpdatedAt { get; init; }
}
