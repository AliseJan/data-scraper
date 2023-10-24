using data_scraper.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace data_scraper.Services;

public interface IStorageService
{
    Task UploadToBlobAsync(string content, string blobName);
    Task<string> ReadFromBlobAsync(string blobName);
    Task SaveToTableAsync(CustomTableEntity entity);
    Task<IEnumerable<CustomTableEntity>> QueryFromTableAsync(string filter);
}