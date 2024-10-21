using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Shift")]
    public class Shift
    {
        [Key]
        public int Id_Shift { get; set; }
        public int TimeSlot { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public List<Employee> Employees { get; } = [];

    }
}
