﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetGrooming_Management_System.Data;

#nullable disable

namespace PetGrooming_Management_System.Migrations
{
    [DbContext(typeof(MainDBContext))]
    [Migration("20241106035246_fixComboServicesTable_02")]
    partial class fixComboServicesTable_02
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PetGrooming_Management_System.Models.Annoucement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Annoucement");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdCustomer")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.AppointmentDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<int?>("ComboId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeWorking")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId")
                        .IsUnique()
                        .HasFilter("[AppointmentId] IS NOT NULL");

                    b.HasIndex("ComboId")
                        .IsUnique()
                        .HasFilter("[ComboId] IS NOT NULL");

                    b.HasIndex("EmployeeId");

                    b.ToTable("AppointmentDetail");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.AppointmentService", b =>
                {
                    b.Property<int?>("AppointmentDetailId")
                        .HasColumnType("int");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("AppointmentDetailId", "ServiceId");

                    b.HasIndex("ServiceId");

                    b.ToTable("AppointmentService");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Combo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Combo");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.ComboServices", b =>
                {
                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.Property<int?>("ComboId")
                        .HasColumnType("int");

                    b.Property<string>("PetName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ServiceId", "ComboId");

                    b.HasIndex("ComboId");

                    b.ToTable("ComboServices");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.EmployeeShift", b =>
                {
                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int?>("ShiftId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("EmployeeId", "ShiftId", "Date");

                    b.HasIndex("ShiftId");

                    b.HasIndex("EmployeeId", "ShiftId", "Date")
                        .IsUnique();

                    b.ToTable("EmployeeShifts");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PetName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PetWeight")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PriceValue")
                        .HasColumnType("float");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Price");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("ServiceName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<TimeOnly?>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("TimeSlot")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Shift");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EndTime = new TimeOnly(12, 0, 0),
                            StartTime = new TimeOnly(8, 0, 0),
                            TimeSlot = 1
                        },
                        new
                        {
                            Id = 2,
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

                    b.Property<string>("Gender")
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

            modelBuilder.Entity("PetGrooming_Management_System.Models.UserAnnouncements", b =>
                {
                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int?>("AnnoucementId")
                        .HasColumnType("int");

                    b.Property<bool>("HasRead")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("UserId", "AnnoucementId");

                    b.HasIndex("AnnoucementId");

                    b.ToTable("UserAnnouncements");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Employee", b =>
                {
                    b.HasBaseType("PetGrooming_Management_System.Models.User");

                    b.Property<bool>("IsWorking")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

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

            modelBuilder.Entity("PetGrooming_Management_System.Models.Appointment", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.User", "Customer")
                        .WithMany("ListAppointments")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.AppointmentDetail", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.Appointment", "Appointment")
                        .WithOne("AppointmentDetail")
                        .HasForeignKey("PetGrooming_Management_System.Models.AppointmentDetail", "AppointmentId");

                    b.HasOne("PetGrooming_Management_System.Models.Combo", "Combo")
                        .WithOne("AppointmentDetail")
                        .HasForeignKey("PetGrooming_Management_System.Models.AppointmentDetail", "ComboId");

                    b.HasOne("PetGrooming_Management_System.Models.Employee", "Employee")
                        .WithMany("AppointmentDetail")
                        .HasForeignKey("EmployeeId");

                    b.Navigation("Appointment");

                    b.Navigation("Combo");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.AppointmentService", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.AppointmentDetail", "AppointmentDetail")
                        .WithMany("AppointmentServices")
                        .HasForeignKey("AppointmentDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PetGrooming_Management_System.Models.Service", "Service")
                        .WithMany("AppointmentServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppointmentDetail");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.ComboServices", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.Combo", "Combo")
                        .WithMany("ComboServices")
                        .HasForeignKey("ComboId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PetGrooming_Management_System.Models.Service", "Service")
                        .WithMany("ComboServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Combo");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.EmployeeShift", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.Employee", "Employee")
                        .WithMany("EmployeeShifts")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PetGrooming_Management_System.Models.Shift", "Shift")
                        .WithMany("EmployeeShifts")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Price", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.Service", "Service")
                        .WithMany("Prices")
                        .HasForeignKey("ServiceId");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.UserAnnouncements", b =>
                {
                    b.HasOne("PetGrooming_Management_System.Models.Annoucement", "Annoucement")
                        .WithMany("UserAnnouncements")
                        .HasForeignKey("AnnoucementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PetGrooming_Management_System.Models.User", "User")
                        .WithMany("UserAnnouncements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Annoucement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Annoucement", b =>
                {
                    b.Navigation("UserAnnouncements");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Appointment", b =>
                {
                    b.Navigation("AppointmentDetail");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.AppointmentDetail", b =>
                {
                    b.Navigation("AppointmentServices");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Combo", b =>
                {
                    b.Navigation("AppointmentDetail");

                    b.Navigation("ComboServices");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Service", b =>
                {
                    b.Navigation("AppointmentServices");

                    b.Navigation("ComboServices");

                    b.Navigation("Prices");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Shift", b =>
                {
                    b.Navigation("EmployeeShifts");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.User", b =>
                {
                    b.Navigation("ListAppointments");

                    b.Navigation("UserAnnouncements");
                });

            modelBuilder.Entity("PetGrooming_Management_System.Models.Employee", b =>
                {
                    b.Navigation("AppointmentDetail");

                    b.Navigation("EmployeeShifts");
                });
#pragma warning restore 612, 618
        }
    }
}
