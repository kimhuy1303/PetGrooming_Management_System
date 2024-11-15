using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<bool> RegisterShift(EmployeeShiftRequest registerShiftdto)
        {
            if (registerShiftdto.Date.Date >= DateTime.Now.Date)
            {
                var existingEmployeeShift = await IsExist(registerShiftdto);
                if (existingEmployeeShift != true)
                {
                    var emloyee = await _employeeRepository.GetEmployeeById(registerShiftdto.EmployeeId);
                    var shift = await _shiftRepository.GetShiftById(registerShiftdto.ShiftId);
                    var assignedShift = new EmployeeShift()
                    {
                        EmployeeId = registerShiftdto.EmployeeId,
                        Employee = emloyee,
                        ShiftId = registerShiftdto.ShiftId,
                        Shift = shift,
                        Date = registerShiftdto.Date.Date,
                    };
                    await _dbcontext.EmployeeShifts.AddAsync(assignedShift);
                    await _dbcontext!.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<ICollection<EmployeeShift>> GetEmployeeShiftsByIdForAWeek(int id, DateTime start, DateTime end)
        {
            var employeeShifts = await _dbcontext.EmployeeShifts.Where(e => e.EmployeeId == id && e.Date.Date >= start.Date && e.Date.Date <= end.Date).ToListAsync();
            return employeeShifts;
        }
        public async Task<EmployeeShift> GetEmployeeShift(int employeeId, DateTime date)
        {
            var employeeShift = await _dbcontext.EmployeeShifts
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId && 
                                          e.Date.Date == date.Date);
            return employeeShift;
        }

        public async Task DeleteEmployeeShift(EmployeeShiftRequest employeeShiftdto)
        {
            var employeeShift = await GetEmployeeShift(employeeShiftdto.EmployeeId, employeeShiftdto.Date);
            if (employeeShift != null) 
            {
                _dbcontext!.EmployeeShifts.Remove(employeeShift);
                _dbcontext!.SaveChanges();
            }
        }

        public async Task UpdateEmployeeShift(EmployeeShiftRequest employeeShiftdto)
        {
            var registeredShift = await GetEmployeeShift(employeeShiftdto.EmployeeId, employeeShiftdto.Date);
            if (registeredShift != null) registeredShift.ShiftId = employeeShiftdto.ShiftId;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<ICollection<EmployeeShift>> GetEmployeeShiftsByDay(DateTime date)
        {
            var res = await _dbcontext.EmployeeShifts.Include(e => e.Employee).Include(e => e.Shift).Where(e => e.Date.Date == date.Date && e.Schedule != null).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsForWeek(DateTime start, DateTime end)
        {
            var result = await _dbcontext.EmployeeShifts.Include(e => e.Employee).Include(e => e.Shift).Where(e => e.Date.Day >= start.Day && e.Date.Day <= end.Day).ToListAsync();

            return result;
        }

        public async Task<int> GetNumberOfEmployeeRegisterShiftForAWeek(DateTime start, DateTime end)
        {
            var result = await GetNotScheduleEmployeeShiftsForWeek(start,end);
            return result.Select(e=>e.EmployeeId).Distinct().Count();
        }

        public async Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(int employeeId)
        {
            return await _dbcontext.EmployeeShifts.Where(e => e.EmployeeId == employeeId).OrderBy(e => e.Date.Date).ToListAsync();
        }

        public async Task<bool> IsExist(EmployeeShiftRequest employeeShiftRequest)
        {
            var check = await _dbcontext.EmployeeShifts.FirstOrDefaultAsync(e => e.EmployeeId == employeeShiftRequest.EmployeeId &&
                                          e.Date.Date == employeeShiftRequest.Date.Date
                                          && e.ShiftId == employeeShiftRequest.ShiftId);
            if (check != null) return true;
            return false;
        }

        public async Task<IEnumerable<EmployeeShiftResponse>> GetNotScheduleEmployeeShiftsForWeek(DateTime start, DateTime end)
        {
            var result = await _dbcontext.EmployeeShifts.Where(e => e.Date.Day >= start.Day && e.Date.Day <= end.Day && e.Schedule.Equals(null))
                                                        .OrderBy(e => e.Date)
                                                        .Select(ems => new EmployeeShiftResponse
                                                        {
                                                            ShiftId = ems.ShiftId,
                                                            TimeSlot = ems.Shift.TimeSlot,
                                                            EmployeeId = ems.EmployeeId,
                                                            EmployeeName = ems.Employee.FullName,
                                                            EmployeeWorkHours = ems.Employee.TotalWorkHours,
                                                            Date = ems.Date.Date

                                                        })
                                                        .ToListAsync();

            return result;
        }

        public async Task<ICollection<EmployeeShiftResponse>> GetNotScheduleEmployeeShiftsByDay(DateTime date)
        {
            var res = await _dbcontext.EmployeeShifts.Where(e => e.Date.Date == date.Date && e.Schedule.Equals(null))
                                                     .Select(ems => new EmployeeShiftResponse
                                                     {
                                                         ShiftId = ems.ShiftId,
                                                         TimeSlot = ems.Shift.TimeSlot,
                                                         EmployeeId = ems.EmployeeId,
                                                         EmployeeName = ems.Employee.FullName,
                                                         EmployeeWorkHours = ems.Employee.TotalWorkHours,
                                                         Date = ems.Date.Date

                                                     })
                                                     .ToListAsync();
            return res;
        }

        public async Task UpdateTotalHoursWorkOfEmployee(int workHour, Employee employee)
        {
            employee.TotalWorkHours += workHour;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<bool> IsEmployeeWorkingByDateInSchedule(int employeeId, DateTime date)
        {
            var employeeShiftByDate = await GetEmployeeShiftsByDay(date);
            var isWorking = employeeShiftByDate.Any(e => e.EmployeeId == employeeId);
            return isWorking;
        }
    }
}
