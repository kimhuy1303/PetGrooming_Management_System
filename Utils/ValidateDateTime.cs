namespace PetGrooming_Management_System.Utils
{
    public class ValidateDateTime
    {
        public bool IsValid(DateTime currentDate, DateTime NewDate) 
        {
            if(currentDate > NewDate) return false;
            return true;
        }
        public int DayRange(DateTime start, DateTime end)
        {
            return end.Day - start.Day;
        }
    }
}
