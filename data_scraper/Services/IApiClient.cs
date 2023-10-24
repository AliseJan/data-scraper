using System.Net.Http;
using System.Threading.Tasks;

namespace data_scraper.Services;

public interface IApiClient
{
    Task<HttpResponseMessage> FetchDataAsync(string url);
}