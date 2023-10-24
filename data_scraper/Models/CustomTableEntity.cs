using Azure;
using Azure.Data.Tables;
using System;

namespace data_scraper.Models;

public class CustomTableEntity : ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string Status { get; set; }
    public DateTimeOffset Date { get; set; }
}