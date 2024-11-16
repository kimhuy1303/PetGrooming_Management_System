using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Repositories
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly MainDBContext _dbContext;
        private readonly IEmployeeRepository _employeeRepository;

        public ShiftRepository(MainDBContext dbContext, IEmployeeRepository employeeRepository) {
            _dbContext = dbContext;
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> CreateShift(ShiftRequest shiftDTO)
        {
            var newShift = new Shift
            {
                TimeSlot = shiftDTO.TimeSlot,
                StartTime = shiftDTO.StartTime,
                EndTime = shiftDTO.EndTime,
            };
            await _dbContext.Shifts.AddAsync(newShift);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteShiftById(int id)
        {
            var shift = await GetShiftById(id);
            _dbContext.Shifts.Remove(shift);
            return await _dbContext.SaveChangesAsync() > 0;
            
        }

        public async Task<ICollection<Shift>> GetAllShifts()
        {
            return await _dbContext.Shifts.ToListAsync();
        }

        public async Task<Shift> GetShiftById(int id)
        {
            return await _dbContext.Shifts.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<int> GetWorkHoursInTimeSlot(int shiftId)
        {
            var shift = await GetShiftById(shiftId);
            int workHours = shift.EndTime.Value.Hour - shift.StartTime.Value.Hour;
            return workHours;
        }

        public async Task<bool> UpdateShift(int id, ShiftRequest shiftDTO)
        {
            var shift = await GetShiftById(id);
            if (shift == null) return false;
            shift.TimeSlot = shiftDTO.TimeSlot;
            shift.StartTime = shiftDTO.StartTime;
            shift.EndTime = shiftDTO.EndTime;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
