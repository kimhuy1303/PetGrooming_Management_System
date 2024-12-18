﻿using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IScheduleRepository
    {
        Task CreateSchedule(ScheduleRequest rawSchedule);
        Task<ScheduleRequest> RawSchedule(DateTime start, DateTime end);
        Task<ScheduleResponse> GetScheduleByWeek(DateTime start, DateTime end);
        Task<Schedule> GetScheduleById(int scheduleId);
        //Task<List<EmployeeShift>> GetListEmployeeShiftInSchedule(int id);
        Task UpdateEmloyeeShiftInSchedule(int scheduleId, EmployeeShiftRequest employeeshiftdto);
        Task RemoveEmployeeShift(int scheduleId, EmployeeShiftRequest employeeshiftdto);
        Task<EmployeeShift> GetEmployeeShiftInSchedule(int scheduleId, EmployeeShiftRequest employeeshiftdto);
        Task<IEnumerable<EmployeeShiftResponse>> GetEmployeeShiftsByEmployee(int employeeId, ScheduleResponse schedule);



    }
}
