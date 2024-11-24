using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        // Navigate
        public List<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
    }
}
