using System.ComponentModel.DataAnnotations;

namespace Cloud_Storage.Models
{
    public class Birder
    {
        [Key]
        public int Birder_Id { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public string? Name { get; set; }
        public string? email { get; set; }
        public string? password { get; set; } = null;

        
     }
}
