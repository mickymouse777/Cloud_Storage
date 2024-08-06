using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Data.Tables;

namespace Cloud_Storage.Models
{
    public class Sighting : ITableEntity
    {
        [Key]
        public int Sighting_Id { get; set; }

        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        //Introduce validation sample
        [Required(ErrorMessage = "Please select a birder.")]
        public int Birder_ID { get; set; } // FK to the Birder who made the sighting

        [Required(ErrorMessage = "Please select a bird.")]
        public int Bird_ID { get; set; } // FK to the Bird being sighted

        [Required(ErrorMessage = "Please select the date.")]
        public DateTime Sighting_Date { get; set; } 

        [Required(ErrorMessage = "Please enter the location.")]
        public string? Sighting_Location { get; set; } 
    }
}
