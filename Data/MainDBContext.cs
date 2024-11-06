using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<EmployeeShift> EmployeeShifts { get; set; }

        public DbSet<Annoucement> Annoucements { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }
        public DbSet<ComboServices> ComboServices { get; set; }
        public DbSet<UserAnnouncements> UserAnnouncements { get; set; }
        
        public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // inheritance of User table
            modelBuilder.Entity<User>()
                .HasDiscriminator<String>("UserType")
                .HasValue<Employee>("Employee")
                .HasValue<Manager>("Manager");

            // Set default value
            modelBuilder.Entity<Employee>()
                .Property(e => e.TotalWorkHours)
                .HasDefaultValue(0);
            modelBuilder.Entity<Employee>()
                .Property(e => e.WorkStatus)
                .HasDefaultValue(false);
            modelBuilder.Entity<Employee>()
                .Property(e => e.IsWorking)
                .HasDefaultValue(false);
            modelBuilder.Entity<UserAnnouncements>()
                .Property(e => e.HasRead)
                .HasDefaultValue(false);

            // Seeding data
            modelBuilder.Entity<Shift>()
                .HasData(
                new Shift {Id=1, TimeSlot = 1, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(12, 0) },
                new Shift {Id=2, TimeSlot = 2, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(17, 0) }
            );

            

            // Many to Many
            // Employee - Shift
            modelBuilder.Entity<EmployeeShift>()
                .HasKey(e => new { e.EmployeeId, e.ShiftId, e.Date });
            modelBuilder.Entity<EmployeeShift>()
                .HasOne(e => e.Employee)
                .WithMany(e => e.EmployeeShifts)
                .HasForeignKey(e => e.EmployeeId);
            modelBuilder.Entity<EmployeeShift>()
                .HasOne(e => e.Shift)
                .WithMany(e => e.EmployeeShifts)
                .HasForeignKey(e => e.ShiftId);
            modelBuilder.Entity<EmployeeShift>()
                .HasIndex(e => new { e.EmployeeId, e.ShiftId, e.Date})
                .IsUnique();

            // User - Announcement
            modelBuilder.Entity<UserAnnouncements>()
                .HasKey(e => new { e.UserId, e.AnnoucementId });
            modelBuilder.Entity<UserAnnouncements>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserAnnouncements)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<UserAnnouncements>()
                .HasOne(e => e.Annoucement)
                .WithMany(e => e.UserAnnouncements)
                .HasForeignKey(e=> e.AnnoucementId);

            // Service - Combo
            modelBuilder.Entity<ComboServices>()
                .HasKey(e => new { e.ServiceId, e.ComboId });
            modelBuilder.Entity<ComboServices>()
                .HasOne(e => e.Service)
                .WithMany(e => e.ComboServices)
                .HasForeignKey(e => e.ServiceId);
            modelBuilder.Entity<ComboServices>()
                .HasOne(e => e.Combo)
                .WithMany(e => e.ComboServices)
                .HasForeignKey(e => e.ComboId);

            // Service - AppointmentDetail
            modelBuilder.Entity<AppointmentService>()
                .HasKey(e => new { e.AppointmentDetailId, e.ServiceId });
            modelBuilder.Entity<AppointmentService>()
                .HasOne(e => e.AppointmentDetail)
                .WithMany (e => e.AppointmentServices)
                .HasForeignKey(e => e.AppointmentDetailId);
            modelBuilder.Entity<AppointmentService>()
                .HasOne(e => e.Service)
                .WithMany(e => e.AppointmentServices)
                .HasForeignKey (e => e.ServiceId);

            modelBuilder.Entity<ComboServices>()
            .HasKey(cs => new { cs.ComboId, cs.ServiceId, cs.PetName, cs.PetWeight });

            base.OnModelCreating(modelBuilder);

        }
    }
}
