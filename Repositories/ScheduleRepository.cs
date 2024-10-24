using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly MainDBContext _dbContext;
        public ScheduleRepository(MainDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task CreateSchedule()
        {
            throw new NotImplementedException();
        }

        public Task EditSchedule(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Schedule>> GetAllSchedules()
        {
            return await _dbContext.Schedules
                .Include(e => e.EmployeeShifts)
                .ToListAsync();
        }

        public async Task<ICollection<Schedule>> GetSchedulesByDate(DateTime date)
        {
            return await _dbContext.Schedules
                .Include(e=> e.EmployeeShifts.Where(e=> e.Date.Date.Equals(date.Date)))
                .ToListAsync();
                
        }

        public async Task<ICollection<Schedule>> GetSchedulesByEmployeeId(int id)
        {
            return await _dbContext.Schedules
                .Include (e=> e.EmployeeShifts.Where(r => r.EmployeeId.Equals(id)))
                .ToListAsync();
        }   
    }
}
