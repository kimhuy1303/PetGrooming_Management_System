using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IEmployeeShiftRepository
    {
        Task<Boolean> RegisterShift(EmployeeShiftRequest registerShiftdto);

        Task<ICollection<EmployeeShift>> GetEmployeeShifts(int id);

        Task<Boolean> DeleteEmployeeShift(EmployeeShiftRequest employeeShiftRequest);
        Task<Boolean> UpdateEmployeeShift(EmployeeShiftRequest registerShiftdto);
    }
}
