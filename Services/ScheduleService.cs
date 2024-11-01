using PetGrooming_Management_System.IRepositories;

namespace PetGrooming_Management_System.Services
{
    public class ScheduleService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeShiftRepository _employeeShiftRepository;

        public ScheduleService(IEmployeeRepository employeeRepository, IShiftRepository shiftRepository, IEmployeeShiftRepository employeeShiftRepository)
        {
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _employeeShiftRepository = employeeShiftRepository;
        }

        
    }
}
