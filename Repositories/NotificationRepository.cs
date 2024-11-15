using Microsoft.EntityFrameworkCore;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using PetGrooming_Management_System.SignalR;
using System.Runtime.InteropServices;

namespace PetGrooming_Management_System.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationHub _notihub;
        private readonly MainDBContext _dbcontext;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        public NotificationRepository(MainDBContext dbcontext, NotificationHub notificationHub, IUserRepository userRepository, IEmployeeRepository employeeRepository, IAppointmentRepository appointmentRepository)
        {
            _dbcontext = dbcontext;
            _notihub = notificationHub;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<Annoucement> Add(string message)
        {
            var announce = new Annoucement
            {
                Content = message,
                CreatedDate = DateTime.UtcNow,
            };
            await _dbcontext.Annoucements.AddAsync(announce);
            await _dbcontext.SaveChangesAsync();
            return announce;
        }

        public async Task SendNotificationToAllEmployees(string message)
        {
            var announce = await Add(message);
            var employees = await _employeeRepository.GetAllEmployees();
            var employeeAnnouncements = employees.Select(employee => new UserAnnouncements
            {
                UserId = employee.Id,
                User = employee,
                Annoucement = announce,
                AnnoucementId = announce.Id,
                HasRead = false,
            }).ToList();
            await _dbcontext.UserAnnouncements.AddRangeAsync(employeeAnnouncements);
            await _dbcontext.SaveChangesAsync();

            await _notihub.SendNotificationToEmployees(message);
        }

        public async Task SendNotificationToCustomer(string message, int customerId, int appointmentId)
        {
            var announce = await Add(message);
            var customerAnnouce = new UserAnnouncements
            {
                UserId = customerId,
                User = await _userRepository.GetUserById(customerId),
                AnnoucementId = announce.Id,
                Annoucement = announce,
                AppointmentId = appointmentId,
                Appointment = await _appointmentRepository.GetAppointmentById(appointmentId),
                HasRead = false
            };
            await _dbcontext.UserAnnouncements.AddAsync(customerAnnouce);
            await _dbcontext.SaveChangesAsync();
            await _notihub.SendNotification(customerId.ToString(), message);
        }

        public async Task SendNotificationToEmployee(string message, int employeeId, int appointmentId)
        {
            var announce = await Add(message);
            var customerAnnouce = new UserAnnouncements
            {
                UserId = employeeId,
                User = await _employeeRepository.GetEmployeeById(employeeId),
                AnnoucementId = announce.Id,
                Annoucement = announce,
                AppointmentId = appointmentId,
                Appointment = await _appointmentRepository.GetAppointmentById(appointmentId),
                HasRead = false
            };
            await _dbcontext.UserAnnouncements.AddAsync(customerAnnouce);
            await _dbcontext.SaveChangesAsync();
            await _notihub.SendNotification(employeeId.ToString(), message);
        }

        public async Task<List<Annoucement>> GetListByUserId(int userId)
        {
            return await _dbcontext.Annoucements.Where(e => e.UserAnnouncements.Any(ua => ua.UserId == userId))
                                          .Include(e => e.UserAnnouncements)
                                          .OrderByDescending(e => e.CreatedDate)
                                          .ToListAsync();

        }

        public async Task MarkAsRead(int annoucementId, int userId)
        {
            var announce = await _dbcontext.UserAnnouncements.FirstOrDefaultAsync(e => e.UserId == userId && e.AnnoucementId == annoucementId);
            if (announce != null)
            {
                announce.HasRead = true;
                await _dbcontext.SaveChangesAsync();
            }
        }
    }
}
