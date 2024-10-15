using PetGrooming_Management_System.Config.Constant;

namespace PetGrooming_Management_System.Models
{
    public class Manager : User
    {
        public Manager() {
            if(Role != Role.Manager) Role = Role.Manager;
        }


    }
}
