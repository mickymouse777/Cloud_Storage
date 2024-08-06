using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cloud_Storage.Models
{
    public class Sighting
    {
        [Key]
        public int Sighting_Id { get; set; }
        [Required]
        public DateOnly Sighting_Date { get; set; }
        [Required]
        public string? Sighting_Location { get; set; }

        [ForeignKey("Birder")]
        public int Birder_Id { get; set; }
        public virtual Birder? Birder { get; set; }

        [ForeignKey("Bird")]
        public int Bird_Id { get; set; }
        public virtual Bird? Bird { get; set; }

    }

}