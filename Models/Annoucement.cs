using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetGrooming_Management_System.Models
{
    [Table("Annoucement")]
    public class Annoucement
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Status { get; set; }

        // Navigate
        public List<UserAnnouncements> UserAnnouncements { get; } = new List<UserAnnouncements>();

    }

}
