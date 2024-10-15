using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Data
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // inheritance of User table
            modelBuilder.Entity<User>()
                .HasDiscriminator<String>("UserType")
                .HasValue<Employee>("Employee")
                .HasValue<Manager>("Manager");

        }
    }
}
