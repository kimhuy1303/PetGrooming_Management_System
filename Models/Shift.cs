using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Shift")]
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        public int TimeSlot { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }

        // Navigate
        public List<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();

    }
}
