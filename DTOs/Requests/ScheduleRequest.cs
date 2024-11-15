using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.DTOs.Requests
{
    public class ScheduleRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<EmployeeShift> EmployeeShifts { get; set; }
    }
}
