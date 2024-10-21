namespace PetGrooming_Management_System.Models
{
    public class EmployeeShift
    {
        public int EmployeeId {  get; set; }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
        public int? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }
    }
}
