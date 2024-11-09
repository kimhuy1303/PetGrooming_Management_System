using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IEmployeeShiftRepository
    {
        Task<Boolean> RegisterShift(EmployeeShiftRequest registerShiftdto);

        Task<ICollection<EmployeeShift>> GetEmployeeShiftsByIdForAWeek(int id, DateTime start, DateTime end);

        Task<Boolean> DeleteEmployeeShift(EmployeeShiftRequest employeeShiftRequest);
        Task<Boolean> UpdateEmployeeShift(EmployeeShiftRequest registerShiftdto);

        Task<ICollection<EmployeeShift>> GetEmployeeShiftsByDay(DateTime date);

        Task<int> GetNumberOfEmployeeRegisterShiftForAWeek(DateTime start, DateTime end);
        Task<IEnumerable<EmployeeShift>> GetEmployeeShiftsForWeek(DateTime start, DateTime end);
        Task<EmployeeShift> GetEmployeeShift(int employeeId, DateTime date);
        Task<Boolean> IsExist(EmployeeShiftRequest employeeShiftRequest);
        Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(int employeeId);
    }
}
