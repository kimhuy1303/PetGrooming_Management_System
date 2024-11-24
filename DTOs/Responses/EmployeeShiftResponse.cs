namespace PetGrooming_Management_System.DTOs.Responses
{
    public class EmployeeShiftResponse
    {
        public int? ShiftId { get; set; }
        public int TimeSlot { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public int EmployeeWorkHours { get; set; }
        public DateTime Date { get; set; }
    }
}
