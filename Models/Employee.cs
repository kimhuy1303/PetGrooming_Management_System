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
        public DateTime DateofHire {  get; set; }
        public int WorkHours { get; set; } = 0;
    }
}
