using PetGrooming_Management_System.Configs.Constant;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public Status Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Navigate
        public int? CustomerId { get; set; }
        public User? Customer { get; set; }
        public AppointmentDetail? AppointmentDetail { get; set; }

    }
}
