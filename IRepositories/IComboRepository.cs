using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IComboRepository
    {
        Task<IEnumerable<object>> GetAllCombos();
        Task<Combo> GetComboById(int id);
        Task<object> DisplayComboById(int id);
        Task<Combo> GetComboByName(string name);
        Task<Combo> CreateCombo(ComboRequest combodto);
        Task<Combo> UpdateCombo(Combo combo, ComboRequest combodto);
        Task DeleteCombo(int id);

        Task<Combo> AddServicesToCombo(ComboServiceRequest listservicesdto);
        Task<Boolean> RemoveServicesFromCombo(int comboId, int servicesId);

    }
}
