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
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        // Navigate
        public List<ComboServices> ComboServices { get; } = new List<ComboServices>();
        public List<AppointmentDetail> AppointmentDetails { get; set; } = new List<AppointmentDetail>();
    }
}
