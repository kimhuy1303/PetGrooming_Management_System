namespace PetGrooming_Management_System.DTOs.Responses
{
    public class ScheduleResponse
    {
        public int ScheduleId { get; set; }
        public List<EmployeeShiftResponse> EmployeeShifts { get; set; } = new();
    }
}
