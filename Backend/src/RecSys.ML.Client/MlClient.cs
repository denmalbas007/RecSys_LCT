using Newtonsoft.Json;

namespace RecSys.ML.Client;

public class MlClient
{
    private readonly HttpClient _client;

    public MlClient(IHttpClientFactory httpClientFactory)
        => _client = httpClientFactory.CreateClient(nameof(MlClient));

    public async Task<IDictionary<string, decimal>> GetMlRangeAsync(byte[] bytes, DateTime[] periods, CancellationToken cancellationToken)
    {
        const string url = "/uploadfile";
#pragma warning disable IDE0028
        var postBody = new MultipartFormDataContent();
#pragma warning restore IDE0028
        postBody
            .Add(new ByteArrayContent(bytes), "file", "file.cvs");
        postBody
            .Add(new StringContent(periods[0].ToString("MM/yyyy")), "periods");
        postBody
            .Add(new StringContent(periods[1].ToString("MM/yyyy")), "periods");
        postBody
            .Add(new StringContent(periods[2].ToString("MM/yyyy")), "periods");
        postBody
            .Add(new StringContent(periods[3].ToString("MM/yyyy")), "periods");
        var result = await _client.PostAsync(url, postBody, cancellationToken);
        result.EnsureSuccessStatusCode();
        var stringRes = await result.Content.ReadAsStringAsync(cancellationToken);
        stringRes = stringRes.Replace("\"", "'");
        stringRes = stringRes.Replace("\\", string.Empty);
        stringRes = stringRes[1..];
        stringRes = stringRes[..^1];
        var rw = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(stringRes);
        return rw!;
    }
}
