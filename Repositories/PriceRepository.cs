using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly MainDBContext _dbcontext;
        public PriceRepository(MainDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Price> CreatePrice(PriceRequest pricedto)
        {

            var newPrice = new Price
            {
                PetName = pricedto.PetName,
                PetWeight = pricedto.PetWeight,
                PriceValue = pricedto.PriceValue,
            };
            await _dbcontext.Prices.AddAsync(newPrice);
            await _dbcontext.SaveChangesAsync();
            return newPrice;
        }

        public async Task AddPrice(int priceid, int serviceid)
        {
            var price = await GetPriceById(priceid);
            price.ServiceId = serviceid;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<ICollection<Price>> GetAllPrices()
        {
            return await _dbcontext.Prices.ToListAsync();
        }

        public async Task<Price> GetPriceById(int id)
        {
            return await _dbcontext.Prices.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Price> IsPriceExist(PriceRequest pricedto)
        {
            var price = await _dbcontext.Prices.FirstOrDefaultAsync(e => e.PetName.Equals(pricedto.PetName) &&
                                                                         e.PetWeight.Equals(pricedto.PetWeight) &&
                                                                         e.PriceValue == pricedto.PriceValue);
            return price;
        }

    }
}
