using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
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

        public async Task<Schedule> GetScheduleByWeek(DateTime start, DateTime end)
        {
            var res = await _dbcontext.Schedules.Include(e => e.EmployeeShifts).FirstOrDefaultAsync(e => e.startDate.Date == start.Date && e.endDate.Date == end.Date);
            return res;
        }
    }
}
