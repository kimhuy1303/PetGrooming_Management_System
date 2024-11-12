namespace PetGrooming_Management_System.DTOs.Requests
{
    public class EmployeeShiftRequest
    {
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
    }

}
