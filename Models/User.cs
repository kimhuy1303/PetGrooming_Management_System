using Microsoft.AspNetCore.Identity;
using PetGrooming_Management_System.Config.Constant;
using System.ComponentModel.DataAnnotations;

namespace PetGrooming_Management_System.Models
{
    public partial class User
    {
        [Key]
        public int Id { get; set; } 
        
        public string? AvatarPath { get; set; }
        [Required]
        [StringLength(200)]
        public string? Username { get; set; }
        [Required]
        [StringLength(200)]
        public string? Password { get; set; }
        public string? FullName { get; set; }
        [StringLength(20)]
        public string? IdentificationNumber { get; set; }
        public string? Email { get; set; }
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public Role Role { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigate
        public List<Appointment>? ListAppointments { get; } = new List<Appointment>();
        public List<UserAnnouncements> UserAnnouncements { get; } = new List<UserAnnouncements>();
    }
}
