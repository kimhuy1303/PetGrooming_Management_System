using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using System.Linq;
using System.Runtime.InteropServices;

namespace PetGrooming_Management_System.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly MainDBContext _dbContext;
        private readonly IPriceRepository _priceRepository;

        public ServiceRepository(MainDBContext dbContext, IPriceRepository priceRepository)
        {
            _dbContext = dbContext;
            _priceRepository = priceRepository;
        }

        public async Task ActiveService(int id)
        {
            var service = await GetServiceById(id);
            service.IsActive = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Service> CreateService(ServiceRequest servicedto)
        {
            var newService = new Service
            {
                ServiceName = servicedto.ServiceName,
                Description = servicedto.Description,
                DateCreated = DateTime.UtcNow,
                IsActive = servicedto.IsActive
            };
            await _dbContext.Services.AddAsync(newService);
            await _dbContext.SaveChangesAsync();
            return newService;
        }

        public async Task<bool> DeleteService(Service service)
        {
            _dbContext.Services.Remove(service);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<Service>> GetAllServices()
        {
            var listServices = await _dbContext.Services.Include(e => e.Prices).ToListAsync();
            return listServices;
        }

        public async Task<double> GetPriceService(int id, string petName, string petWeight)
        {
            var price = await GetServiceByPet(id, petName, petWeight);
            return price.Prices.FirstOrDefault().PriceValue;
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await _dbContext.Services
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Service> GetServiceByName(string name)
        {
            return await _dbContext.Services
                .Include(e => e.Prices)
                .FirstOrDefaultAsync(e => e.ServiceName.Equals(name));
        }

        public async Task<Service> GetServiceByPet(int id, string petName, string petWeight)
        {
            return await _dbContext.Services
                .Include(e => e.Prices
                               .Where(e => e.PetName.Equals(petName)
                                     && e.PetWeight.Equals(petWeight)))
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        
        public async Task<ICollection<ServiceResponse>> GetServicesByPet(string petName, string petWeight)
        {
            var services = await _dbContext.Services
                            .Where(price => price.Prices.Any(e => e.PetName == petName && e.PetWeight == petWeight))
                            .Include(x => x.Prices.Where(price => price.PetName == petName && price.PetWeight == petWeight).Take(1))
                            .ToListAsync();
            var res = services.Select(s => new ServiceResponse
            {
                ServiceId = s.Id,
                ServiceName = s.ServiceName,
                Price = s.Prices.FirstOrDefault().PriceValue
            }).ToList();
            return res;
        }

        public async Task<bool> IsServiceExist(int id, PriceRequest pricedto)
        {
            var price = await _priceRepository.IsPriceExist(pricedto);
            bool isExist = await _dbContext.Services
                .Where(e => e.Id == id)
                .AnyAsync(e => e.Prices.Any(e => e.Id == price.Id));
            return isExist;
        }

        public async Task RemovePrice(Service service, Price price)
        {
            service.Prices.Remove(price);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Service>> SearchService(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Service>> SortService(string sortString)
        {
            throw new NotImplementedException();

        }

        public async Task<Service> UpdateService(Service service, ServiceRequest servicedto)
        {
            service.ServiceName = servicedto.ServiceName;
            service.Description = servicedto.Description;
            service.IsActive = servicedto.IsActive;
            //await _priceRepository.AddPrice(service.Id,servicedto.PriceRequest);
            await _dbContext.SaveChangesAsync();
            return service;
        }
    }
}
