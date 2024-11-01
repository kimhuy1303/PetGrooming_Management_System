

using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IServiceRepository
    {
        Task<ICollection<Service>> GetAllServices();
        Task<Service> GetServiceById(int id);
        Task<Boolean> DeleteService(Service service);
        Task<Boolean> UpdateService(Service service, ServiceRequest servicedto);
        Task<Boolean> CreateService(ServiceRequest servicedto);
        Task<ICollection<Service>> SearchService(string key);
    }
}
