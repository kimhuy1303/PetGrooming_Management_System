using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.DTOs.Responses
{
    public class AppointmentDetailResponse
    {
        public int AppointmentId { get; set; }
        public int AppointmentDetailId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public string? AppointmentStatus { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string PetName { get; set; }
        public string PetWeight { get; set; }
        public string? ComboName { get; set; }
        public List<ServicesResponse> Services { get; set; } = new List<ServicesResponse>();
        public DateTime BookingTime { get; set; }
        public double TotalPrice { get; set; }
    }
    public class AppointmentResponse
    {
        public int AppointmentId { get; set; }
        public string? CustomerName { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public double TotalPrice { get; set; }
        public string? AppointmentStatus { get; set; }
        
    }
    public class ServicesResponse
    {
        public string ServiceName { get; set; }
        public double Price { get; set; }
    }
    public class PriceResponse
    {
        public string? PetName { get; set; }
        public string? PetWeight { get; set; }
        public double Price { get; set; }
    }
}
