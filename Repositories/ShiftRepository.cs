using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Respones;
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

        public async Task<ICollection<Shift>> GetAllShifts()
        {
            return await _dbContext.Shifts.ToListAsync();
        }

        public async Task<Shift> GetShiftById(int id)
        {
            return await _dbContext.Shifts.FirstOrDefaultAsync(e => e.Id == id);
        }

    }
}
