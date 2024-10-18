namespace PetGrooming_Management_System.DTOs.Requests
{
    public class ProfileRequest
    {
        public string? AvatarPath { get; set; }
        public string? FullName { get; set; }
        public string? IdentificationNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
    }
}
