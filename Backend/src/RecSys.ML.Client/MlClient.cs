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
}
