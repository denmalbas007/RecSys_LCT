using System.Net.Http.Json;

namespace RecSys.Customs.Client;

public class CustomsClient
{
    private readonly HttpClient _client;

    public CustomsClient(IHttpClientFactory httpClientFactory)
        => _client = httpClientFactory.CreateClient(nameof(CustomsClient));

    public async Task<Stream?> UnloadDataAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        const string url = "api/DataAnalysis/UnloadData";
        var request = new UnloadDataRequest(periods: new[] { new Period { Start = from, End = to } });
        var result = await _client.PostAsJsonAsync(url, request, cancellationToken);
        var stream = await result.Content.ReadAsStreamAsync(cancellationToken);
        return stream.Length == 205 ? null : stream;
    }

    public async Task<ItemType[]> GetRootItemTypesAsync(CancellationToken cancellationToken)
    {
        const string url = "api/Tnved/GetRootCodes?level=4";
        var result = await _client.GetFromJsonAsync<ItemType[]>(url, cancellationToken: cancellationToken);
        return result!;
    }

    public async Task<ItemType[]> GetItemTypesAsync(string parent, CancellationToken cancellationToken)
    {
        var url = $"api/Tnved/GetCodes?parent={parent}";
        var result = await _client.GetFromJsonAsync<ItemType[]>(url, cancellationToken: cancellationToken);
        return result!;
    }
}
