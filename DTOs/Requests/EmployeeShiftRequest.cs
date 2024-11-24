namespace PetGrooming_Management_System.DTOs.Requests
{
    public class EmployeeShiftRequest
    {
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
    }

    public class ShiftRequests
    {
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
    }
    public class RegisterShiftRequest
    {
        public int EmployeeId { get; set; }
        public List<ShiftRequests> ShiftRequests { get; set; }
    }

}
