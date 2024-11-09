using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using System.Runtime.InteropServices;

namespace PetGrooming_Management_System.Services
{
    public class ScheduleService
    {

        private readonly int _employeePerSchedule = 3; // số nhân viên trong mỗi ca
        private readonly IEmployeeShiftRepository _employeeShiftRepository;
        private List<EmployeeShift> _employeeShifts;

        public ScheduleService(IEmployeeShiftRepository employeeShiftRepository)
        {
            _employeeShiftRepository = employeeShiftRepository;
        }

        public async Task<List<EmployeeShift>> GenerateSchedule(DateTime start, DateTime end) 
        {
            _employeeShifts.Clear();
            await Backtrack(start, end);
            return _employeeShifts;
        }

        private async Task<bool> Backtrack(DateTime start, DateTime end)
        {
            if (start > end) return true;  
            var registeredShifts = await _employeeShiftRepository.GetEmployeeShiftsForWeek(start, end);
            var shiftsForDay = registeredShifts.Where(e => e.Date.Date == start.Date);
            foreach (var timeSlot in new[] { 1, 2}) // Ca sáng, chiều, full ngày
            {
                var availableEmployees = await Task.Run(() =>
                {
                    return shiftsForDay.Where(s => s.Shift.TimeSlot == timeSlot)
                                       .GroupBy(e => e.Employee.TotalWorkHours)
                                       .OrderBy(g => g.Key)
                                       .FirstOrDefault().ToList();
                });
                if (availableEmployees != null && availableEmployees.Count >= 3)
                {
                    var selectedShifts = availableEmployees.OrderBy(s => Guid.NewGuid()).Take(3).ToList();
                    _employeeShifts.AddRange(selectedShifts);
                }
            }
               
            return await Backtrack(start.AddDays(1), end);
        }
        
    }
}
