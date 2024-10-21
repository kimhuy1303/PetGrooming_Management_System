using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Data
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions<MainDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // inheritance of User table
            modelBuilder.Entity<User>()
                .HasDiscriminator<String>("UserType")
                .HasValue<Employee>("Employee")
                .HasValue<Manager>("Manager");

            // Set default value
            modelBuilder.Entity<Employee>()
                .Property(e => e.TotalAppointment)
                .HasDefaultValue(0);
            modelBuilder.Entity<Employee>()
                .Property(e => e.TotalWorkHours)
                .HasDefaultValue(0);
            modelBuilder.Entity<Employee>()
                .Property(e => e.WorkStatus)
                .HasDefaultValue(false);
            modelBuilder.Entity<Employee>()
                .Property(e => e.IsWorking)
                .HasDefaultValue(false);

            // Seeding data
            modelBuilder.Entity<Shift>()
                .HasData(
                new Shift {Id_Shift=1, TimeSlot = 1, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(12, 0) },
                new Shift {Id_Shift=2, TimeSlot = 2, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(17, 0) }
            );

            // Many to Many
            // Employee - Shift
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Shifts)
                .WithMany(e => e.Employees)
                .UsingEntity<EmployeeShift>(
                    r => r.HasOne<Shift>().WithMany().HasForeignKey(e => e.ShiftId),
                    l => l.HasOne<Employee>().WithMany().HasForeignKey(e => e.EmployeeId)
                );
        }
    }
}
