using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Combo")]
    public class Combo
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }

        // Navigate
        public List<Service> ListServices { get; } = new List<Service>();
        public List<ComboServices> ComboServices { get; } = new List<ComboServices>();
        public AppointmentDetail? AppointmentDetail { get; set; }
    }
}
