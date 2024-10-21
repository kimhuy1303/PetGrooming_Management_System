﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetGrooming_Management_System.Data;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    [DbContext(typeof(MainDBContext))]
    partial class MainDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PetGrooming_Management_System.Models.Appointment", b =>
                {
                    b.Property<int>("Id_Appointment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Appointment"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Appointment");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.EmployeeShift", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ShiftId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ScheduleId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId", "ShiftId");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("ShiftId");

                    b.ToTable("EmployeeShifts");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Schedule", b =>
                {
                    b.Property<int>("Id_Schedule")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Schedule"));

                    b.HasKey("Id_Schedule");

                    b.ToTable("Schedule");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Shift", b =>
                {
                    b.Property<int>("Id_Shift")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_Shift"));

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("TimeSlot")
                        .HasColumnType("int");

                    b.HasKey("Id_Shift");

                    b.ToTable("Shift");

                    b.HasData(
                        new
                        {
                            Id_Shift = 1,
                            EndTime = new TimeOnly(12, 0, 0),
                            StartTime = new TimeOnly(8, 0, 0),
                            TimeSlot = 1
                        },
                        new
                        {
                            Id_Shift = 2,
                            EndTime = new TimeOnly(17, 0, 0),
                            StartTime = new TimeOnly(13, 0, 0),
                            TimeSlot = 2
                        });
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AvatarPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentificationNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("UserType").HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Employee", b =>
                {
                    b.HasBaseType("PetGrooming_Management_System.Models.User");

                    b.Property<bool>("IsWorking")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("TotalAppointment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("TotalWorkHours")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("WorkStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Manager", b =>
                {
                    b.HasBaseType("PetGrooming_Management_System.Models.User");

                    b.HasDiscriminator().HasValue("Manager");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.EmployeeShift", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PetGrooming_Management_System.Models.Schedule", "Schedule")
                        .WithMany("EmployeeShifts")
                        .HasForeignKey("ScheduleId");

                    b.HasOne("PetGrooming_Management_System.Models.Shift", null)
                        .WithMany()
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Schedule", b =>
                {
                    b.Navigation("EmployeeShifts");
                });
#pragma warning restore 612, 618
        }
    }
}
