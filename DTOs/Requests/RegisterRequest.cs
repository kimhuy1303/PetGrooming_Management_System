namespace PetGrooming_Management_System.DTOs.Requests
{
    public class RegisterRequest
    {
        public string Username  { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
