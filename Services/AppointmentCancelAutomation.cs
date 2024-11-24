
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.Configs.Constant;

namespace PetGrooming_Management_System.Services
{
    public class AppointmentCancelAutomation : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AppointmentCancelAutomation(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _dbContext = scope.ServiceProvider.GetRequiredService<MainDBContext>();
                    var now = DateTime.UtcNow;

                    var expiredAppointments = _dbContext.Appointments.Where(e => e.Status == Status.Pending.ToString() &&
                                                                                 e.AppointmentDetail!.TimeWorking.AddMinutes(10) < now)
                                                                     .ToList();
                    foreach (var appointment in expiredAppointments)
                    {
                        appointment.Status = Status.Canceled.ToString();
                    }
                    await _dbContext.SaveChangesAsync(stoppingToken);
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
