namespace PetGrooming_Management_System.DTOs.Requests
{
    public class ServiceRequest
    {
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        
    }

    public class PriceRequest
    {
        public string? PetName { get; set; }
        public string? PetWeight { get; set; }
        public double PriceValue { get; set; }
    }
}
