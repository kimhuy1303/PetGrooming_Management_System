using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Service")]
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool IsActive { get; set; }

        // Navigate
        public List<ComboServices> ComboServices { get; } = new List<ComboServices> { };
        
        public List<AppointmentService> AppointmentServices { get; } = new List<AppointmentService> { };
        public List<Price> Prices { get;  } = new List<Price> { };

    }
}
