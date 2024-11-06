using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.DTOs.Respones
{
    public class ServicesResponse
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
        public List<Price>? Prices { get; set; }
        
    }

    public class ComboRespone
    {
        public List<Combo> Combos { get; set; }
        public double Price { get; set; }
    }
}
