using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Configs.Constant;
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
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public AppointmentRepository(MainDBContext dbcontext, IComboRepository comboRepository, IUserRepository userRepository, IEmployeeRepository employeeRepository)
        {
            _dbcontext = dbcontext;
            _comboRepository = comboRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;

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

        public async Task<AppointmentDetail> MakeAnAppointment(int customerId, AppointmentRequest appointmentdto)
        {
            var appointment = await CreateAppointment(appointmentdto, customerId);
            var appointmentDetail = await CreateAppointmentDetail(appointment.Id, appointmentdto.AppointmentDetail);

            // Nếu có dịch vụ và không có combo
            if (!appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId == 0) 
            {
                
                foreach(AppointmentServicesRequest service in appointmentdto.AppointmentDetail.AppointmentServices)
                {
                    await AddServicesToAppointment(appointmentDetail.Id, service);
                }
            // Nếu có combo và không có dịch vụ
            }else if (appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId != 0)
            {
                appointmentDetail.ComboId = appointmentdto.AppointmentDetail.comboId;
                await _dbcontext.SaveChangesAsync();
            }
            // Nếu có cả combo và dịch vụ
            else if(!appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId != 0)
            {
                foreach (AppointmentServicesRequest service in appointmentdto.AppointmentDetail.AppointmentServices)
                {
                    await AddServicesToAppointment(appointmentDetail.Id, service);
                }
                appointmentDetail.ComboId = appointmentdto.AppointmentDetail.comboId;
                await _dbcontext.SaveChangesAsync();
            }
            return appointmentDetail;
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
            var appointment = await GetAppointmentById(appointmentId);
            if (appointment != null)
            {
                appointment.Name = appointmentdto.CustomerName;
                appointment.PhoneNumber = appointmentdto.CustomerPhoneNumber;
                appointment.Email = appointmentdto.CustomerEmail;
                appointment.Address = appointmentdto.CustomerAddress;
                if(Enum.TryParse(typeof(Status), appointmentdto.StatusString, true, out var parsedStatus))
                {
                    appointment.Status = (Status)parsedStatus;
                }
            }
            await _dbcontext.SaveChangesAsync();
            var appointmentDetail = await _dbcontext.AppointmentDetails
                                                    .FirstOrDefaultAsync(e => e.AppointmentId == appointmentId);
            if (appointmentDetail != null) 
            {
                var newEmployee = await _employeeRepository.GetEmployeeById(appointmentdto.AppointmentDetail.EmployeeId);
                var newCombo = await _comboRepository.GetComboById((int)appointmentdto.AppointmentDetail.comboId);
                appointmentDetail.EmployeeId = newEmployee.Id;
                appointmentDetail.Employee = newEmployee;
                appointmentDetail.ComboId = appointmentDetail.ComboId;
                appointmentDetail.Combo = newCombo;
                appointmentDetail.TimeWorking = appointmentdto.AppointmentDetail.TimeWorking;
            }
            await _dbcontext.SaveChangesAsync();
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
            };
            if (customerId != 0) 
            {
                newAppointment.CustomerId = customerId;
                newAppointment.Customer = await _userRepository.GetUserById(customerId);
            }
            await _dbcontext.Appointments.AddAsync(newAppointment);
            await _dbcontext.SaveChangesAsync();
            return newAppointment;
        }

        public async Task ChangeStatusAppointment(Appointment appointment, string status)
        {
            if (Enum.TryParse(typeof(Status), status, true, out var parsedStatus))
            {
                appointment.Status = (Status)parsedStatus;
            }
            await _dbcontext.SaveChangesAsync();
        }
    }
}
