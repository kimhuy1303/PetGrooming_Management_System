using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IShiftRepository
    {
        Task<Shift> GetShiftById(int id);
        Task<ICollection<Shift>> GetAllShifts();
        
    }
}
