namespace PetGrooming_Management_System.Utils
{
    public class ValidateDateTime
    {
        public bool IsValid(DateTime currentDate, DateTime NewDate) 
        {
            if(currentDate > NewDate) return false;
            return true;
        }
    }
}
