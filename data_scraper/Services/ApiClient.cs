using System.Net.Http;
using System.Threading.Tasks;

namespace data_scraper.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<HttpResponseMessage> FetchDataAsync(string url)
    {
        return await _httpClient.GetAsync(url);
    }
}