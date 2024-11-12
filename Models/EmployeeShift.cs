namespace PetGrooming_Management_System.Models
{
    public class EmployeeShift
    {
        // Navigate
        public int? EmployeeId {  get; set; }
        public Employee? Employee { get; set; }
        public int? ShiftId { get; set; }
        public Shift? Shift { get; set; }
        public DateTime Date { get; set; }
        public int? ScheduleId { get; set; }
        public Schedule? Schedule { get; set; }

    }
}

