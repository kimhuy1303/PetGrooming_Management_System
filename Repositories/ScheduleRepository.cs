using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using PetGrooming_Management_System.Services;

namespace PetGrooming_Management_System.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly MainDBContext _dbcontext;
        private readonly ScheduleService _scheduleService;
       
        public ScheduleRepository(ScheduleService scheduleService, MainDBContext mainDBContext)
        {
            _scheduleService = scheduleService;
            _dbcontext = mainDBContext;
        }

        public async Task UpdateEmloyeeShiftInSchedule(int scheduleId, EmployeeShiftRequest employeeshiftdto)
        {
            var employeeshift = await _dbcontext.EmployeeShifts.FirstOrDefaultAsync(e => e.ScheduleId == scheduleId && e.EmployeeId == employeeshiftdto.EmployeeId && e.Date.Date == employeeshiftdto.Date.Date && e.ShiftId == employeeshiftdto.ShiftId)
            if (employeeshift != null) 
            {
                employeeshift.ScheduleId = scheduleId;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task CreateSchedule(DateTime start, DateTime end)
        {
            var schedule = new Schedule
            {
                startDate = start,
                endDate = end,
                EmployeeShifts = await _scheduleService.GenerateSchedule(start, end)
            };
            await _dbcontext.Schedules.AddAsync(schedule);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<EmployeeShift>> GetListEmployeeShiftInSchedule(int scheduleId)
        {
            var listEmployeeShifts = await _dbcontext.EmployeeShifts.Where(e => e.ScheduleId == scheduleId)
                                                                    .Include(e => e.Employee)
                                                                    .Include(e => e.Shift)
                                                                    .ToListAsync();
            return listEmployeeShifts;                                                   
        }

        public async Task<Schedule> GetScheduleById(int scheduleId)
        {
            return await _dbcontext.Schedules.FirstOrDefaultAsync(e => e.Id == scheduleId);
        }

        public async Task<Schedule> GetScheduleByWeek(DateTime start, DateTime end)
        {
            var res = await _dbcontext.Schedules.Include(e => e.EmployeeShifts).FirstOrDefaultAsync(e => e.startDate.Date == start.Date && e.endDate.Date == end.Date);
            return res;
        }

        public async Task RemoveEmployeeShift(int scheduleId, EmployeeShiftRequest employeeshiftdto)
        {
            var employeeShift = await GetEmployeeShiftInSchedule(scheduleId, employeeshiftdto);
            if(employeeShift != null) {
                employeeShift.Schedule = null;
                employeeShift.ScheduleId = null;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<EmployeeShift> GetEmployeeShiftInSchedule(int scheduleId, EmployeeShiftRequest employeeshiftdto)
        {
            return await _dbcontext.EmployeeShifts.FirstOrDefaultAsync(e => e.ScheduleId == scheduleId && e.EmployeeId == employeeshiftdto.EmployeeId && e.Date.Date == employeeshiftdto.Date.Date && e.ShiftId == employeeshiftdto.ShiftId);
        }
    }
} 
