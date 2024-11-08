using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Repositories
{
    public class EmployeeShiftRepository : IEmployeeShiftRepository
    {
        private readonly MainDBContext _dbcontext;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        
        public EmployeeShiftRepository(MainDBContext dbcontext, IEmployeeRepository employeeRepository, IShiftRepository shiftRepository)
        {
            _dbcontext = dbcontext;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;

        }
        public async Task<Boolean> RegisterShift(EmployeeShiftRequest registerShiftdto)
        {
            if (registerShiftdto.Date.Date >= DateTime.Now.Date)
            {
                var existingEmployeeShift = await _dbcontext.EmployeeShifts
                    .FirstOrDefaultAsync(e => e.ShiftId == registerShiftdto.IdShift && e.EmployeeId == registerShiftdto.IdEmployee && e.Date == registerShiftdto.Date);
                if (existingEmployeeShift == null)
                {
                    var shift = await _shiftRepository.GetShiftById(registerShiftdto.IdShift);
                    var employee = await _employeeRepository.GetEmployeeById(registerShiftdto.IdEmployee);
                    var assignedShift = new EmployeeShift()
                    {
                        Employee = employee,
                        Shift = shift,
                        Date = registerShiftdto.Date.Date,
                    };
                    employee.EmployeeShifts.Add(assignedShift);
                    await _dbcontext!.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<ICollection<EmployeeShift>> GetEmployeeShifts(int id)
        {
            var employeeShifts = await _dbcontext.EmployeeShifts.Where(e => e.EmployeeId == id).ToListAsync();
            return employeeShifts;
        }
        public async Task<EmployeeShift> GetEmployeeShift(EmployeeShiftRequest employeeShiftdto)
        {
            var employeeShift = await _dbcontext.EmployeeShifts
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeShiftdto.IdEmployee && 
                                          e.ShiftId == employeeShiftdto.IdShift && 
                                          e.Date == employeeShiftdto.Date);
            return employeeShift;
        }

        public async Task<bool> DeleteEmployeeShift(EmployeeShiftRequest employeeShiftdto)
        {
            var employeeShift = await GetEmployeeShift(employeeShiftdto);
            if (employeeShift != null) 
            {
                _dbcontext!.EmployeeShifts.Remove(employeeShift);
                _dbcontext!.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateEmployeeShift(EmployeeShiftRequest employeeShiftdto)
        {
            // Handling
            return true;
        }

        public async Task<ICollection<EmployeeShift>> GetEmployeeShiftsByDay(int day)
        {
            var res = await _dbcontext.EmployeeShifts.Where(e => e.Date.Day == day).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsForWeek(DateTime start, DateTime end)
        {
            var result = await _dbcontext.EmployeeShifts.Where(e => e.Date.Day >= start.Day && e.Date.Day <= end.Day).ToListAsync();

            return result;
        }

        public async Task<int> GetNumberOfEmployeeRegisterShiftForAWeek(DateTime start, DateTime end)
        {
            var result = await GetEmployeeShiftsForWeek(start,end);
            return result.Count();
        }
    }
}
