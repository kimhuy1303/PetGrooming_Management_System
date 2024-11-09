using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IScheduleRepository
    {
        Task CreateSchedule(DateTime start, DateTime end);
        Task<Schedule> GetScheduleByWeek(DateTime start, DateTime end);
    }
}
