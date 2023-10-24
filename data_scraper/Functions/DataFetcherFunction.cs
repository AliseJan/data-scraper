using data_scraper.Models;
using data_scraper.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace data_scraper.Functions
{
    public class DataFetcherFunction
    {
        private readonly IApiClient _apiClient;
        private readonly IStorageService _storageService;

        public DataFetcherFunction(IApiClient apiClient, IStorageService storageService)
        {
            _apiClient = apiClient;
            _storageService = storageService;
        }

        [FunctionName("FetchAndStoreData")]
        public async Task FetchAndStoreData([TimerTrigger("0 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"Function executed at: {DateTime.UtcNow}");

                var result = await _apiClient.FetchDataAsync("https://api.publicapis.org/random?auth=null");
                log.LogInformation($"{result}");

                var logEntity = new CustomTableEntity
                {
                    PartitionKey = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    RowKey = Guid.NewGuid().ToString(),
                    Date = DateTime.UtcNow
                };

                if (result.IsSuccessStatusCode)
                {
                    logEntity.Status = "Success";

                    var contentAsString = await result.Content.ReadAsStringAsync();

                    await _storageService.UploadToBlobAsync(contentAsString, logEntity.RowKey);
                }
                else
                {
                    logEntity.Status = "Failure";
                }

                await _storageService.SaveToTableAsync(logEntity);
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}