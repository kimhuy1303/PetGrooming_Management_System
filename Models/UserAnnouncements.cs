namespace PetGrooming_Management_System.Models
{
    public class UserAnnouncements
    {
        // Navigate
        public int? UserId { get; set; }
        public User? User { get; set; }
        
        public int? AnnoucementId { get; set; }
        public Annoucement? Annoucement { get; set; }

        public Boolean HasRead { get; set; }
    }
}
