using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        // Navigate
        public List<EmployeeShift> EmployeeShifts { get; } = new List<EmployeeShift>(); 

    }
}
