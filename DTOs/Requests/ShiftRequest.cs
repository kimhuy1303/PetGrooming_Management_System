namespace PetGrooming_Management_System.DTOs.Requests
{
    public class ShiftRequest
    {
        public int TimeSlot { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
    }
}
