using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IPriceRepository
    {
        Task<Price> CreatePrice(PriceRequest pricedto);
        Task<Price> GetPriceById(int id);
        Task<ICollection<Price>> GetAllPrices();
        Task<Price> IsPriceExist(PriceRequest pricedto);
        Task AddPrice(int priceid, int serviceid);

        Task<ICollection<PetResponse>> GetPets(); 

    }
}
