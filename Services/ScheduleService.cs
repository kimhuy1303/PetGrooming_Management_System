using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using System.Runtime.InteropServices;

namespace PetGrooming_Management_System.Services
{
    public class ScheduleService
    {

        private readonly int _employeePerSchedule = 3; // số nhân viên trong mỗi ca
        private readonly IEmployeeShiftRepository _employeeShiftRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IShiftRepository _shiftRepository;
        private List<EmployeeShift> _employeeShifts = new List<EmployeeShift>();
        private Dictionary<int, int> tempWorkHours = new Dictionary<int, int>(); 

        public ScheduleService(IEmployeeShiftRepository employeeShiftRepository, IEmployeeRepository employeeRepository, IShiftRepository shiftRepository)
        {
            _employeeShiftRepository = employeeShiftRepository;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
        }

        public async Task<List<EmployeeShift>> GenerateSchedule(DateTime start, DateTime end) 
        {
            if(_employeeShifts != null) _employeeShifts.Clear();
            await Backtrack(start, end);
            return _employeeShifts.OrderBy(e => e.Date).ToList();
        }

        private async Task<bool> Backtrack(DateTime start, DateTime end)
        {
            if (start > end) return true;  
            //var registeredShifts = await _employeeShiftRepository.GetNotScheduleEmployeeShiftsForWeek(start, end);
            var shiftsForDay = await _employeeShiftRepository.GetNotScheduleEmployeeShiftsByDay(start);
            foreach(var employeeShift in shiftsForDay)
            {
                tempWorkHours[(int)employeeShift.EmployeeId] = employeeShift.EmployeeWorkHours;
            }
            
            foreach (var timeSlot in new[] { 1, 2}) // Ca sáng, chiều, full ngày
            {
                var availableEmployees = await Task.Run(() =>
                {
                    return shiftsForDay.Where(s => s.TimeSlot == timeSlot)
                                       .OrderBy(e => tempWorkHours[e.EmployeeId ?? 0])
                                       .ToList();
                });
                if (availableEmployees != null)
                {
                    var selectedShifts = availableEmployees.OrderBy(s => Guid.NewGuid()).Take(3).Select(e => new EmployeeShift
                    {
                        ShiftId = e.ShiftId,
                        EmployeeId = e.EmployeeId,
                        Date = e.Date.Date
                    }).ToList();
                    _employeeShifts.AddRange(selectedShifts);
                    foreach(var employeeShift in selectedShifts)
                    {
                        var employee = await _employeeRepository.GetEmployeeById((int)employeeShift.EmployeeId);
                        var shift = await _shiftRepository.GetShiftById((int)employeeShift.ShiftId);
                        int workHours = shift.EndTime.Value.Hour - shift.StartTime.Value.Hour;
                        if (tempWorkHours.ContainsKey(employee.Id))
                        {
                            tempWorkHours[employee.Id] += workHours;
                        }
                    }
                }
            }     
            return await Backtrack(start.AddDays(1), end);
        }
    }
}
