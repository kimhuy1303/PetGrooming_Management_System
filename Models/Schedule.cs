using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Schedule")]
    public class Schedule
    {
        [Key]
        public int Id_Schedule { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsApprove { get; set; }

    }
}
