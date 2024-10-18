using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        public int Id_Appointment { get; set; }
        public string? Name { get; set; }
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
