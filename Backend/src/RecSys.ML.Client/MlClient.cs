using System.Net.Http.Json;

namespace RecSys.ML.Client;

public class MlClient
{
    private readonly HttpClient _client;

    public MlClient(IHttpClientFactory httpClientFactory)
        => _client = httpClientFactory.CreateClient(nameof(MlClient));

    public async Task<IDictionary<string, decimal>> GetMlRangeAsync(MemoryStream stream, DateTime[] periods, CancellationToken cancellationToken)
    {
        const string url = "/uploadfile";
#pragma warning disable IDE0028
        var postBody = new MultipartFormDataContent();
#pragma warning restore IDE0028
        postBody
            .Add(new ByteArrayContent(stream.ToArray()), "file_data", "file.cvs");
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
        var resultDict = await result.Content.ReadFromJsonAsync<Dictionary<string, decimal>>(cancellationToken: cancellationToken);
        return resultDict!;
    }
}
