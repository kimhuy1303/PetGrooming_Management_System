using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface INotificationRepository
    {
        public Task SendNotificationToCustomer(string message, int customerId, int appointmentId);
        public Task SendNotificationToEmployee(string message, int employeeId, int appointmentId);
        public Task SendNotificationToAllEmployees(string message);
        public Task<Annoucement> Add(string message);
        Task<List<Annoucement>> GetListByUserId(int userId);
        Task MarkAsRead(int annoucementId, int userId);
        Task<IEnumerable<UserAnnouncements>> GetAnnouncementsUnread(int userId);

    }
}
