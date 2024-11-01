namespace PetGrooming_Management_System.Models
{
    public class ComboServices
    {
        public int? ComboId { get; set; }
        public Combo? Combo { get; set; }

        public int? ServiceId { get; set; }
        public Service? Service { get; set; }
        public double Price { get; set; }
    }
}
