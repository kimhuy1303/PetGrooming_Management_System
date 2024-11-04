

using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IServiceRepository
    {
        Task<ICollection<Service>> GetAllServices();
        Task<Service> GetServiceById(int id);
        Task<Service> GetServiceByName(string name);
        Task<Service> GetServiceByPet(int id, string petName, string petWeight);
        Task<ICollection<Service>> GetServicesByPet(string petName, string petWeight);
        Task<Boolean> DeleteService(Service service);
        Task<Service> UpdateService(Service service, ServiceRequest servicedto);
        Task<Service> CreateService(ServiceRequest servicedto);
        Task<ICollection<Service>> SearchService(string key);

        Task ActiveService(int id);
        Task<ICollection<Service>> SortService(string sortString);
        Task<Boolean> IsServiceExist(int id, PriceRequest pricedto);
        

    }
}
