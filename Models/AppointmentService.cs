namespace PetGrooming_Management_System.Models
{
    public class AppointmentService
    {
        public string PetName { get; set; }
        public string PetWeight { get; set; }
        public double Price { get; set; }
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        public int? AppointmentDetailId { get; set; }
        public AppointmentDetail? AppointmentDetail { get; set; }
    }
}
