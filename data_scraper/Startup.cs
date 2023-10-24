using data_scraper.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(data_scraper.Startup))]

namespace data_scraper
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddSingleton<IConfiguration>(configuration);
            builder.Services.AddScoped<IApiClient, ApiClient>();
            builder.Services.AddSingleton<IStorageService, AzureStorageService>();
        }
    }
}