using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace RecSys.ML.Client;

public class MlClient
{
    private readonly HttpClient _client;

    public MlClient(IHttpClientFactory httpClientFactory)
        => _client = httpClientFactory.CreateClient(nameof(MlClient));

    public async Task<IDictionary<string, decimal>> GetMlRangeAsync(byte[] bytes, DateTime[] periods, CancellationToken cancellationToken)
    {
        var newperiods = periods.ToList();
        newperiods.Sort();
        const string url = "/uploadfile";
#pragma warning disable IDE0028
        var postBody = new MultipartFormDataContent();
#pragma warning restore IDE0028
        postBody
            .Add(new ByteArrayContent(bytes), "file", "file.cvs");
        foreach (var per in newperiods)
        {
            postBody
                .Add(new StringContent(per.ToString("MM/yyyy")), "periods");
        }

        var result = await _client.PostAsync(url, postBody, cancellationToken);
        result.EnsureSuccessStatusCode();
        var stringRes = await result.Content.ReadAsStringAsync(cancellationToken);
        stringRes = stringRes.Replace("\"", "'");
        stringRes = stringRes.Replace("\\", string.Empty);
        stringRes = stringRes[1..];
        stringRes = stringRes[..^1];
        var rw = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(stringRes);
        var qqwe = rw!.DistinctBy(x => x.Value).Take(50).ToDictionary(x => x.Key, y => y.Value);
        return qqwe;
    }

    public async Task<IDictionary<long, byte[]>> GetGraphsAsync(
        IDictionary<long, IDictionary<long, decimal>> data,
        CancellationToken cancellationToken)
    {
        var dict = new Dictionary<long, byte[]>();
        const string url = "/render-graph";
        foreach (var dat in data)
        {
            var tnkved = new List<long>();
            for (var i = 1; i <= dat.Value.Take(5).Count(); i++)
            {
                tnkved.Add(i);
            }

            var request = new DataForGraph()
            {
                Tnkved = tnkved.ToArray(),
                Cooefs = dat.Value.Select(x => x.Value).Take(5).ToArray()
            };
            var result = await _client.PostAsJsonAsync(url, request, cancellationToken);
            var base64 = await result.Content.ReadAsStringAsync(cancellationToken);
            dict.Add(dat.Key, Convert.FromBase64String(base64.Replace("\"", string.Empty)));
        }

        return dict;
    }

    public class DataForGraph
    {
        [JsonPropertyName("tkved")]
        public long[] Tnkved { get; init; } = Array.Empty<long>();

        [JsonPropertyName("coofs")]
        public decimal[] Cooefs { get; init; } = Array.Empty<decimal>();
    }
}
