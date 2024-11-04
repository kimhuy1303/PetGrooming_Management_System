using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Repositories
{
    public class ComboRepository : IComboRepository
    {
        private readonly MainDBContext _dbContext;
        private readonly IServiceRepository _serviceRepository;
        public ComboRepository(MainDBContext dbContext, IServiceRepository serviceRepository) {
            _dbContext = dbContext;
            _serviceRepository = serviceRepository;
        }


        public async Task<Combo> AddServicesToCombo(ComboServiceRequest listservicesdto)
        {
            var combo = await GetComboById(listservicesdto.ComboId);
            var listService = new List<Service>();
            
            foreach(int serviceId in listservicesdto.ListServicesId)
            {
                var service = await _serviceRepository.GetServiceByPet(serviceId, listservicesdto.PetName, listservicesdto.PetWeight);
                if (service != null)
                {
                    listService.Add(service);
                }
            }
            foreach(Service s in listService)
            {
                var totalPrice = s.Prices
                    .Sum(p => p.PriceValue);
                var comboService = new ComboServices
                {
                    ComboId = combo.Id,
                    ServiceId = s.Id,
                    Price = totalPrice,
                };
                combo.ComboServices.Add(comboService);
            }
            await DiscountCombo(combo, listservicesdto.ListServicesId);
            await _dbContext.SaveChangesAsync();
            return combo;
        }

        public async Task<Combo> CreateCombo(ComboRequest combodto)
        {
            var newCombo = new Combo
            {
                Name = combodto.ComboName,
                CreatedDate = DateTime.Now,
                IsActive = false,
            };
            await _dbContext.Combos.AddAsync(newCombo);
            await _dbContext.SaveChangesAsync();
            return newCombo;
        }

        public async Task DeleteCombo(int id)
        {
            var combo = await GetComboById(id);
            _dbContext.Combos.Remove(combo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DiscountCombo(Combo combo, List<int> numberOfServices)
        {
            var discount = 0;
            switch (numberOfServices.Count())
            {
                case 2:
                    discount = 10;
                    break;
                case 3:
                    discount = 15;
                    break;
                case 4:
                    discount = 20;
                    break;
                default:
                    discount = 0;
                    break;

            }
            var comboService = combo.ComboServices.Find(e => e.ComboId == combo.Id);
            comboService.Price = comboService.Price -  comboService.Price*(discount / 100);
        }

        public async Task<ICollection<Combo>> GetAllCombos()
        {
            return await _dbContext.Combos.Include(e => e.ComboServices).ToListAsync();
        }

        public async Task<Combo> GetComboById(int id)
        {
            return await _dbContext.Combos.Include(e => e.ComboServices).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Combo> GetComboByName(string name)
        {
            return await _dbContext.Combos.Include(e => e.ComboServices).FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<Combo> UpdateCombo(Combo combo, ComboRequest combodto)
        {
            combo.CreatedDate = DateTime.Now;
            combo.Name = combodto.ComboName;
            combo.IsActive = combodto.IsActive;
            await _dbContext.SaveChangesAsync();
            return combo;   
        }
    }
}
