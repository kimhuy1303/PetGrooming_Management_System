namespace PetGrooming_Management_System.DTOs.Responses
{
    public class ComboResponse
    {
        public int ComboId { get; set; } 
        public string ComboName { get; set; }      
        public DateTime CreatedDate { get; set; }  
        public bool IsActive { get; set; }
        public List<ComboServiceGroupResponse> ComboServices { get; set; } = new();
    }
    public class ComboServiceGroupResponse
    {
        public string PetName { get; set; }          
        public string PetWeight { get; set; }           
        public double TotalPrice { get; set; }         
        public double DiscountPrice { get; set; }       
        public List<ServiceResponse> Services { get; set; } = new();
    }
    public class ServiceResponse
    {
        public int ServiceId { get; set; }        
        public string ServiceName { get; set; }   
        public double Price { get; set; }
    }
}
