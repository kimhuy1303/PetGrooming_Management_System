using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IEmployeeShiftRepository
    {
        Task<Boolean> RegisterShift(RegisterShiftRequest registerShiftdto);

        Task<ICollection<EmployeeShift>> GetEmployeeShiftsByIdForAWeek(int id, DateTime start, DateTime end);

        Task DeleteEmployeeShift(EmployeeShiftRequest employeeShiftRequest);
        Task UpdateEmployeeShift(EmployeeShiftRequest registerShiftdto);

        Task<ICollection<EmployeeShift>> GetEmployeeShiftsByDay(DateTime date);
        Task<ICollection<EmployeeShiftResponse>> GetNotScheduleEmployeeShiftsByDay(DateTime date);

        Task<int> GetNumberOfEmployeeRegisterShiftForAWeek(DateTime start, DateTime end);
        Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsForWeek(DateTime start, DateTime end);
        Task<IEnumerable<EmployeeShiftResponse>> GetNotScheduleEmployeeShiftsForWeek(DateTime start, DateTime end);
        Task<EmployeeShift> GetEmployeeShift(int employeeId, DateTime date);
        Task<Boolean> IsExist(EmployeeShiftRequest employeeShiftRequest);
        Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(int employeeId);
        Task UpdateTotalHoursWorkOfEmployee(int workHour, Employee employee);
        Task<Boolean> IsEmployeeWorkingByDateInSchedule(int employeeId, DateTime date);

    }
}
