using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using PetGrooming_Management_System.Utils;

namespace PetGrooming_Management_System.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MainDBContext? _dbcontext;
        public EmployeeRepository(MainDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddEmployee(EmployeeProfileRequest employeeDTO)
        {

            var _employee = new Employee
            {
                AvatarPath = UploadFile.GetFilePath(employeeDTO.AvatarPath!),
                Username = employeeDTO.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(employeeDTO.Password),
                FullName = employeeDTO.FullName,
                PhoneNumber = employeeDTO.PhoneNumber,
                IdentificationNumber = employeeDTO.IdentificationNumber,
                Address = employeeDTO.Address,
                Email = employeeDTO.Email,
                Role = Config.Constant.Role.Employee,
                CreatedDate = DateTime.UtcNow,
            };
            await _dbcontext!.Employees.AddAsync(_employee);
            await _dbcontext.SaveChangesAsync();
            
        }

        public async Task DeleteEmployee(int id)
        {
            var _employee = await GetEmployeeById(id);
            _dbcontext!.Remove(_employee);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<ICollection<Employee>> GetAllEmployees()
        {
            return await _dbcontext!.Employees.Include(employee => employee.Shifts).ToListAsync();
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _dbcontext!.Employees.Include(employee=> employee.Shifts).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Employee> GetEmployeeByIdenNumber(string idenNumber)
        {
            return await _dbcontext!.Employees.Include(employee => employee.Shifts).FirstOrDefaultAsync(x => x.IdentificationNumber.Equals(idenNumber));
        }

        public async Task ModifyProfileEmployee(int id, EmployeeProfileRequest profile)
        {
            var _employee = await GetEmployeeById(id);
            _employee.AvatarPath = UploadFile.GetFilePath(profile.AvatarPath!);
            _employee.DateOfBirth = profile.DateOfBirth;
            _employee.Address = profile.Address;
            _employee.PhoneNumber = profile.PhoneNumber;
            _employee.Email = profile.Email;
            _employee.IdentificationNumber = profile.IdentificationNumber;
            await _dbcontext!.SaveChangesAsync();
        }

        public async Task RegisterShift(RegisterShiftRequest registerShiftdto)
        {
            var assignedShift = new EmployeeShift
            {
                EmployeeId = registerShiftdto.IdEmployee,
                ShiftId = registerShiftdto.IdShift,
                Date = registerShiftdto.Date
            };
            await _dbcontext!.EmployeeShifts.AddAsync(assignedShift);
            await _dbcontext!.SaveChangesAsync();
        }
    }
}
