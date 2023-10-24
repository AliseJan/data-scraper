using data_scraper.Models;
using data_scraper.Services;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class AzureStorageService : IStorageService
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly TableClient _tableClient;

    public AzureStorageService(IConfiguration configuration)
    {
        _blobContainerClient = new BlobContainerClient(configuration["AzureWebJobsStorage"], "payloads");
        _tableClient = new TableClient(configuration["AzureWebJobsStorage"], "logs");
    }

    public async Task UploadToBlobAsync(string content, string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)), overwrite: true);
    }

    public async Task<string> ReadFromBlobAsync(string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        using var stream = await blobClient.OpenReadAsync();
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    public async Task SaveToTableAsync(CustomTableEntity entity)
    {
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task<IEnumerable<CustomTableEntity>> QueryFromTableAsync(string filter)
    {
        List<CustomTableEntity> entities = new List<CustomTableEntity>();
        await foreach (var entity in _tableClient.QueryAsync<CustomTableEntity>(filter))
        {
            entities.Add(entity);
        }
        return entities;
    }
}