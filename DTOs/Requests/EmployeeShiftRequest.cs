﻿namespace PetGrooming_Management_System.DTOs.Requests
{
    public class EmployeeShiftRequest
    {
        public int IdEmployee { get; set; }
        public int IdShift { get; set; }
        public DateTime Date { get; set; }
    }
}
