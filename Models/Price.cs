using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Price")]
    public class Price
    {
        [Key]
        public int Id { get; set; }
        public double PriceValue { get; set; }
        public string? PetName { get; set; }
        public string? PetWeight {  get; set; }

        // Navigate
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
