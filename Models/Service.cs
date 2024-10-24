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
        public string? PetName {  get; set; }
        public string? WeightPet { get; set; }
        public double Price { get; set; } 
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsActive { get; set; }

        // Navigate
        public List<ComboServices> ComboServices { get; } = new List<ComboServices> { };
        public int? AppointmentDetailId { get; set; }
        public AppointmentDetail? AppointmentDetail { get; set; }

    }
}
