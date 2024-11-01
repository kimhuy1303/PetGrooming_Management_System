namespace PetGrooming_Management_System.Models
{
    public class AppointmentService
    {
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        public int? AppointmentDetailId { get; set; }
        public AppointmentDetail? AppointmentDetail { get; set; }
    }
}
