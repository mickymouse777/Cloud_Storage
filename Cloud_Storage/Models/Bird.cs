using Azure;
using Azure.Data.Tables;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cloud_Storage.Models
{
    public class Bird : ITableEntity
    {
        [Key]
        public int Bird_Id { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? ImageUrl { get; set; }

        // ITableEntity properties
        public ETag ETag { get; set; } = ETag.All;
        public DateTimeOffset? Timestamp { get; set; }
    }
}
