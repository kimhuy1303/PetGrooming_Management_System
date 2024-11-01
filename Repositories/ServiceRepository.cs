using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly MainDBContext _dbContext;

        public ServiceRepository(MainDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateService(ServiceRequest servicedto)
        {
            var newService = new Service
            {
                ServiceName = servicedto.ServiceName,
                Description = servicedto.Description,
                DateCreated = DateTime.UtcNow,
                IsActive = false
            };
            await _dbContext.Services.AddAsync(newService);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteService(Service service)
        {
            _dbContext.Services.Remove(service);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<ICollection<Service>> GetAllServices()
        {
            var listServices = await _dbContext.Services.ToListAsync(); 
            return listServices;
        }

        public async Task<Service> GetServiceById(int id)
        {
            return await _dbContext.Services.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<ICollection<Service>> SearchService(string key)
        {
            return await _dbContext.Services.Where(e => e.ServiceName.Contains(key)).ToListAsync();
        }

        public async Task<bool> UpdateService(Service service, ServiceRequest servicedto)
        {
            service.ServiceName = servicedto.ServiceName;
            service.Description = servicedto.Description;
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
