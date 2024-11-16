﻿using PetGrooming_Management_System.Configs.Constant;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.DTOs.Requests
{
    public class AppointmentRequest
    {
        public string? CustomerName { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public string? StatusString { get; set; }
        public AppointmentDetailRequest? AppointmentDetail { get; set; }
    }

    public class AppointmentDetailRequest
    {
        public DateTime TimeWorking { get; set; }
        public int? comboId { get; set; }
        public int EmployeeId { get; set; }
        public List<AppointmentServicesRequest>? AppointmentServices { get; set; }
    }

    public class AppointmentServicesRequest
    {
        public int ServiceId { get; set; }
    }
}
