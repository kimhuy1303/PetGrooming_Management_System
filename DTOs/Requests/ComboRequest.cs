namespace PetGrooming_Management_System.DTOs.Requests
{
    public class ComboRequest
    {
        public string? ComboName { get; set; }
    }

    public class ComboServiceRequest
    {
        public int ComboId { get; set; }
        public List<int>? ListServicesId { get; set; }
    }
}
