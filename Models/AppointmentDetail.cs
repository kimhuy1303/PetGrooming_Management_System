using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("AppointmentDetail")]
    public class AppointmentDetail
    {
        [Key]
        public int Id {  get; set; }   
        public DateTime TimeWorking { get; set; }

        // Navigate
        public int? AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int? ComboId { get; set; }
        public Combo? Combo { get; set; }
        public List<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();

    }
}
