using data_scraper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace data_scraper.Functions
{
    public class DataQueryFunctions
    {
        private readonly IStorageService _storageService;
        private readonly ILogger<DataQueryFunctions> _logger;

        public DataQueryFunctions(IStorageService storageService, ILogger<DataQueryFunctions> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        [FunctionName("GetLogs")]
        public async Task<IActionResult> GetLogs(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("GetLogs function processing a request.");
            try
            {
                string fromTime = req.Query["from"];
                string toTime = req.Query["to"];

                if (!DateTime.TryParse(fromTime, out DateTime fromDate) || !DateTime.TryParse(toTime, out DateTime toDate))
                    return new BadRequestObjectResult("Invalid 'from' or 'to' parameters. They should be in DateTime format.");

                var filter = $"Date ge datetime'{fromDate:yyyy-MM-ddTHH:mm:ssZ}' and Date le datetime'{toDate:yyyy-MM-ddTHH:mm:ssZ}'";

                var logs = await _storageService.QueryFromTableAsync(filter);

                return new OkObjectResult(JsonConvert.SerializeObject(logs));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new ObjectResult($"Error: {ex.Message}") { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }

        [FunctionName("GetPayload")]
        public async Task<IActionResult> GetPayload(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("GetPayload function processing a request.");

            string logId = req.Query["logId"];
            if (string.IsNullOrEmpty(logId))
                return new BadRequestObjectResult("Missing 'logid' parameter.");

            try
            {
                var content = await _storageService.ReadFromBlobAsync(logId);
                return new OkObjectResult(content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching the payload: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}