using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IShiftRepository
    {
        Task<Shift> GetShiftById(int id);
        Task<ICollection<Shift>> GetAllShifts();

        Task<Boolean> CreateShift(ShiftRequest shiftDTO); 
        Task<Boolean> DeleteShiftById(int id);
        Task<Boolean> UpdateShift(int id, ShiftRequest shiftDTO);

        
    }
}
