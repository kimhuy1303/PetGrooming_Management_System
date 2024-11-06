namespace PetGrooming_Management_System.DTOs.Requests
{
    public class ComboRequest
    {
        public string? ComboName { get; set; }
        public bool IsActive { get; set; }
    }

    

    public class ComboServiceRequest
    {
        public int ComboId { get; set; }
        public List<int>? ListServicesId { get; set; }
        public string? PetName { get; set; }
        public string? PetWeight { get; set; }
        
    }
}
