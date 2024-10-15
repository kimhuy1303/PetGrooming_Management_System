using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Shift")]
    public class Shift
    {
        [Key]
        public string Id { get; set; }
    }
}
