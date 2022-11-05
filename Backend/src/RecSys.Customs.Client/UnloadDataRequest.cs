using System.Text.Json.Serialization;

namespace RecSys.Customs.Client;

public class UnloadDataRequest
{
    public UnloadDataRequest()
    {
    }

    public UnloadDataRequest(Period[] periods)
        => Periods = periods;

    [JsonPropertyName("exportType")]
    public string ExportType { get; init; } = "Csv";

    [JsonPropertyName("tnved")]
    public string[] ItemTypes { get; init; } = Array.Empty<string>();

    [JsonPropertyName("tnvedLevel")]
    public int ItemTypesLevels { get; init; } = 10;

    [JsonPropertyName("federalDistricts")]
    public string[] FederalDistricts { get; init; } = Array.Empty<string>();

    [JsonPropertyName("subjects")]
    public string[] Subjects { get; init; } = Array.Empty<string>();

    [JsonPropertyName("direction")]
    public string Direction { get; init; } = string.Empty;

    [JsonPropertyName("period")]
    public Period[] Periods { get; init; } = Array.Empty<Period>();
}

public class Period
{
    [JsonPropertyName("start")]
    public string StartFormatted { get; init; } = null!;

    [JsonPropertyName("end")]
    public string EndFormatted { get; init; } = null!;

    [JsonIgnore]
    public DateTime Start
    {
        get => DateTime.Parse(StartFormatted);
        init => StartFormatted = value.ToString("yyyy-MM-dd");
    }

    [JsonIgnore]
    public DateTime End
    {
        get => DateTime.Parse(StartFormatted);
        init => EndFormatted = value.ToString("yyyy-MM-dd");
    }
}

// {
//     "exportType": "Csv",
//     "tnved": [],
//     "tnvedLevel": 10,
//     "federalDistricts": [],
//     "subjects": [],
//     "direction": "",
//     "period": [
//     {
//         "start": "2019-01-01",
//         "end": "2019-01-31"
//     }
//     ]
// }
