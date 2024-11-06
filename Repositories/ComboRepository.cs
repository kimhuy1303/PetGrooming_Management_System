using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using System.Collections.Immutable;

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
                var service = await _serviceRepository.GetServiceById(serviceId);
                if (service != null)
                {
                    listService.Add(service);
                }
            }
            foreach(Service s in listService)
            {
                var price = s.Prices.FirstOrDefault(p => p.PetName == listservicesdto.PetName
                                                      && p.PetWeight == listservicesdto.PetWeight);
                var comboService = new ComboServices
                {
                    Combo = combo,
                    Service = s,
                    PetName = listservicesdto.PetName,
                    PetWeight = listservicesdto.PetWeight,
                    Price = price.PriceValue
                };
                combo.ComboServices.Add(comboService);
            }
            
            await _dbContext.SaveChangesAsync();
            return combo;
        }

        public async Task<Combo> CreateCombo(ComboRequest combodto)
        {
            var newCombo = new Combo
            {
                Name = combodto.ComboName,
                CreatedDate = DateTime.Now,
                IsActive = combodto.IsActive,
                
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
        public async static Task<double> DiscountCombo(int numberOfServices)
        {
            double discount = 0.0;
            switch (numberOfServices)
            {
                case 2:
                    discount = 0.1;
                    break;
                case 3:
                    discount = 0.15;
                    break;
                case 4:
                    discount = 0.2;
                    break;
                default:
                    discount = 0.0;
                    break;

            }
            return discount;
        }
        public async Task<IEnumerable<object>> GetAllCombos()
        {

            return await _dbContext.Combos
                .Select(e => new
                {
                    ComboId = e.Id,
                    ComboName = e.Name,
                    CreatedDate = e.CreatedDate,
                    IsActive = e.IsActive,
                    ComboServices = e.ComboServices
                                .GroupBy(cs => new { cs.PetName, cs.PetWeight })
                                .Select(g => new
                                {
                                    PetName = g.Key.PetName,
                                    PetWeight = g.Key.PetWeight,
                                    TotalPrice = g.Sum(cs => cs.Price),
                                    DiscountPrice = g.Sum(cs => cs.Price) * (1.0 - DiscountCombo(g.Count()).Result),
                                    Service =  g.Select(cs => new
                                    {
                                        ServiceId = cs.ServiceId,
                                        ServiceName = cs.Service.ServiceName,
                                        Price = cs.Price,
                                    }).ToList()
                                }).ToList()
                })
                .ToListAsync();
        }

        public async Task<Combo> GetComboById(int id)
        {
            return await _dbContext.Combos.Include(e => e.ComboServices).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Combo> GetComboByName(string name)
        {
            return await _dbContext.Combos.Include(e => e.ComboServices).FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<bool> RemoveServicesFromCombo(int comboId, int servicesId)
        {
            var comboService = await _dbContext.ComboServices.FirstOrDefaultAsync(e => e.ComboId == comboId && e.ServiceId == servicesId);
            if (comboService != null)
            {
                _dbContext.ComboServices.Remove(comboService);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Combo> UpdateCombo(Combo combo, ComboRequest combodto)
        {
            combo.CreatedDate = DateTime.Now;
            combo.Name = combodto.ComboName;
            combo.IsActive = combodto.IsActive;
            await _dbContext.SaveChangesAsync();
            return combo;   
        }

        public async Task<object> DisplayComboById(int id)
        {
            return await _dbContext.Combos
                .Where(c => c.Id == id)
                .Select(e => new
                {
                    ComboId = e.Id,
                    ComboName = e.Name,
                    CreatedDate = e.CreatedDate,
                    IsActive = e.IsActive,
                    ComboServices = e.ComboServices
                                .GroupBy(cs => new { cs.PetName, cs.PetWeight })
                                .Select(g => new
                                {
                                    PetName = g.Key.PetName,
                                    PetWeight = g.Key.PetWeight,
                                    TotalPrice = g.Sum(cs => cs.Price),
                                    DiscountPrice = g.Sum(cs => cs.Price) * (1.0 - DiscountCombo(g.Count()).Result),
                                    Service = g.Select(cs => new
                                    {
                                        ServiceId = cs.ServiceId,
                                        ServiceName = cs.Service.ServiceName,
                                        Price = cs.Price,
                                    }).ToList()
                                }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
