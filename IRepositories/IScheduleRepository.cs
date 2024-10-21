using PetGrooming_Management_System.Models;
namespace PetGrooming_Management_System.IRepositories
{
    public interface IScheduleRepository
    {
        Task<ICollection<Schedule>> GetSchedulesByDate(DateTime date);
        Task<ICollection<Schedule>> GetSchedulesByEmployeeId(int id);
        Task<ICollection<Schedule>> GetAllSchedules();
    }
}
