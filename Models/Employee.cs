namespace PetGrooming_Management_System.Models
{
    public class Employee : User
    {

        public Employee() { 
            if(Role != Config.Constant.Role.Employee)
            {
                Role = Config.Constant.Role.Employee;
            }
        }
        public int TotalAppointment { get; set; }
        public int TotalWorkHours { get; set; } 
        public bool WorkStatus { get; set; }
        public bool IsWorking { get; set; }

        // Navigate
        public List<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
        public List<AppointmentDetail> AppointmentDetail { get; } = new List<AppointmentDetail>();

    }
}
