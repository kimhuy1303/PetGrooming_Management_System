using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.DTOs.Respones
{
    public class EmployeeShiftRespone
    {
        public int IdEmployee { get; }
        public Shift? Shift {  get; }
        public DateTime Date {  get; }

    }
}
