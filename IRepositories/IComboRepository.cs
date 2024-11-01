using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IComboRepository
    {
        Task<ICollection<Combo>> GetAllCombos();
        Task<Combo> GetComboById(int id);
        Task<Boolean> CreateCombo(ComboRequest combodto);
        Task<Boolean> UpdateCombo(Combo combo, ComboRequest combodto);
        Task<Boolean> DeleteCombo(int id);

        Task<Boolean> AddServicesToCombo(ComboServiceRequest listservicesdto);

    }
}
