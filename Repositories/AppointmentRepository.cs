using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MainDBContext _dbcontext;
        private readonly IComboRepository _comboRepository;
        public AppointmentRepository(MainDBContext dbcontext, IComboRepository comboRepository)
        {
            _dbcontext = dbcontext;
            _comboRepository = comboRepository;
        }

        public async Task AddServicesToAppointment(int appointmentDetailId, AppointmentServicesRequest appointmentservicesdto)
        {
            var appointmentService = new AppointmentService
            {
                AppointmentDetailId = appointmentDetailId,
                ServiceId = appointmentservicesdto.ServiceId
            };
            await _dbcontext.AppointmentServices.AddAsync(appointmentService);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task CancelAppointment(int appointmentId)
        {
            var appointment = await GetAppointmentById(appointmentId);
            appointment.Status = Configs.Constant.Status.Canceled;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task MakeAnAppointment(int customerId, AppointmentRequest appointmentdto)
        {
            var appointment = await CreateAppointment(appointmentdto, customerId);
            var appointmentDetail = await CreateAppointmentDetail(appointment.Id, appointmentdto.AppointmentDetail);

            if (!appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId == 0) 
            {
                foreach(AppointmentServicesRequest service in appointmentdto.AppointmentDetail.AppointmentServices)
                {
                    await AddServicesToAppointment(appointmentDetail.Id, service);
                }
            }else if (appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId != 0)
            {
                appointmentDetail.ComboId = appointmentdto.AppointmentDetail.comboId;
                await _dbcontext.SaveChangesAsync();
            }
            else if(!appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId != 0)
            {
                foreach (AppointmentServicesRequest service in appointmentdto.AppointmentDetail.AppointmentServices)
                {
                    await AddServicesToAppointment(appointmentDetail.Id, service);
                }
                appointmentDetail.ComboId = appointmentdto.AppointmentDetail.comboId;
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<AppointmentDetail> CreateAppointmentDetail(int appointmentId, AppointmentDetailRequest appointmentdetaildto)
        {
            var appointmentDetail = new AppointmentDetail
            {
                TimeWorking = appointmentdetaildto.TimeWorking,
                AppointmentId = appointmentId,
            };
            await _dbcontext.AppointmentDetails.AddAsync(appointmentDetail);
            await _dbcontext.SaveChangesAsync();
            return appointmentDetail;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointments()
        {
            var result = await _dbcontext.Appointments
                                         .Include(e => e.AppointmentDetail)
                                         .ThenInclude(detail => detail.AppointmentServices)
                                         .Include(e => e.AppointmentDetail)
                                         .ThenInclude(detail => detail.Employee)
                                         .Include(e => e.Customer)
                                         .ToListAsync();
            return result;
        }

        public async Task<Appointment> GetAppointmentById(int id)
        {
            var appointment = await _dbcontext.Appointments
                                              .Include(e => e.AppointmentDetail)
                                              .ThenInclude(detail => detail.AppointmentServices)
                                              .Include(e => e.AppointmentDetail)
                                              .ThenInclude(detail => detail.Employee)
                                              .Include(e => e.Customer)
                                              .FirstOrDefaultAsync(e => e.Id == id);
            return appointment;
        }

        public async Task UpdateAppointment(int appointmentId, AppointmentRequest appointmentdto)
        {
            throw new NotImplementedException();
        }

        public async Task<object> ViewAppointmentDetail(int appointmentId)
        {
            var appointmentDetail = await _dbcontext.AppointmentDetails
                                                    .Where(e => e.AppointmentId == appointmentId)
                                                    .Include(e => e.Appointment)
                                                    .ThenInclude(appointment => appointment.Customer)
                                                    .Include(e => e.Employee)
                                                    .Include(e => e.Combo)
                                                    .Include(e => e.AppointmentServices)
                                                    .ThenInclude(s => s.Service)
                                                    .Select(e => new
                                                    {
                                                        AppointmentDetailId = e.Id,
                                                        Timing = e.TimeWorking.TimeOfDay,
                                                        Date = e.TimeWorking.Date,
                                                        AppointmentContact = e.Appointment,
                                                        Groomer = e.Employee.FullName,
                                                        Combo = e.Combo,
                                                        Services = e.AppointmentServices.Select(s => new
                                                        {
                                                            ServiceName = s.Service.ServiceName,
                                                            Price = s.Service.Prices.Select(e => new
                                                            {
                                                                PetName = e.PetName,
                                                                PetWeight = e.PetWeight,
                                                                Price = e.PriceValue
                                                            }).ToList()
                                                        }).ToList()
                                                    }).FirstOrDefaultAsync();
            return appointmentDetail;
        }

        public async Task<Appointment> CreateAppointment(AppointmentRequest appointmentdto, int customerId)
        {
            var newAppointment = new Appointment
            {
                Name = appointmentdto.CustomerName,
                PhoneNumber = appointmentdto.CustomerPhoneNumber,
                Email = appointmentdto.CustomerEmail,
                Address = appointmentdto.CustomerAddress,
                CreatedDate = DateTime.UtcNow,
                Status = Configs.Constant.Status.Pending,
                CustomerId = customerId
            };
            await _dbcontext.Appointments.AddAsync(newAppointment);
            await _dbcontext.SaveChangesAsync();
            return newAppointment;
        }
    }
}
