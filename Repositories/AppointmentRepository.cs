using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Configs.Constant;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Responses;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;

namespace PetGrooming_Management_System.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MainDBContext _dbcontext;
        private readonly IComboRepository _comboRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IServiceRepository _serviceRepository;
        public AppointmentRepository(MainDBContext dbcontext, IComboRepository comboRepository, IUserRepository userRepository, IEmployeeRepository employeeRepository, IServiceRepository serviceRepository)
        {
            _dbcontext = dbcontext;
            _comboRepository = comboRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _serviceRepository = serviceRepository;

        }

        public async Task AddServicesToAppointment(AppointmentDetail appointmentDetaildto, AppointmentServicesRequest appointmentservicesdto)
        {
            var price = await _serviceRepository.GetPriceService(appointmentservicesdto.ServiceId, appointmentDetaildto.PetName, appointmentDetaildto.PetWeight);
            var appointmentService = new AppointmentService
            {
                
                AppointmentDetailId = appointmentDetaildto.Id,
                PetName = appointmentDetaildto.PetName,
                PetWeight = appointmentDetaildto.PetWeight,
                Price = price,
                ServiceId = appointmentservicesdto.ServiceId
            };
            await _dbcontext.AppointmentServices.AddAsync(appointmentService);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<AppointmentDetail> MakeAnAppointment(AppointmentRequest appointmentdto)
        {
            var appointment = await CreateAppointment(appointmentdto, (int)appointmentdto.CustomerId);
            var appointmentDetail = await CreateAppointmentDetail(appointment.Id, appointmentdto.AppointmentDetail);

            // Nếu có dịch vụ và không có combo
            if (!appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId == 0) 
            {
                
                foreach(AppointmentServicesRequest service in appointmentdto.AppointmentDetail.AppointmentServices)
                {
                    await AddServicesToAppointment(appointmentDetail, service);
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
                    await AddServicesToAppointment(appointmentDetail, service);
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
                PetWeight = appointmentdetaildto.PetWeight,
                PetName = appointmentdetaildto.PetName,
                AppointmentId = appointmentId,
            };
            if(appointmentdetaildto.EmployeeId != 0)
            {
                appointmentDetail.EmployeeId = appointmentdetaildto.EmployeeId;
            }
            await _dbcontext.AppointmentDetails.AddAsync(appointmentDetail);
            await _dbcontext.SaveChangesAsync();
            return appointmentDetail;
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAllAppointments(int page, int size)

        {
            var appointments = await _dbcontext.Appointments.AsNoTracking()
                                               .Include(a => a.Customer).Include(a => a.AppointmentDetail)
                                               .Skip((page - 1) * size)
                                               .Take(size).ToListAsync();

            var employeeDetails = await _dbcontext.Employees.AsNoTracking()
                                                            .Where(e => appointments.Select(a => a.AppointmentDetail.EmployeeId).Contains(e.Id))
                                                            .ToListAsync();

            var appointmentServiceDetails = await _dbcontext.AppointmentServices.AsNoTracking()
                                                                               .Include(es => es.Service).ThenInclude(s => s.Prices)
                                                                               .Where(es => appointments.Select(a => a.AppointmentDetail.Id).Contains((int)es.AppointmentDetailId))
                                                                               .ToListAsync();
            var comboDetails = await _dbcontext.Combos.AsNoTracking()
                                                      .Include(c => c.ComboServices)
                                                      .ThenInclude(cs => cs.Service)
                                                      .Where(c => appointments.Select(a => a.AppointmentDetail.ComboId).Contains(c.Id))
                                                      .ToListAsync();
            var result = appointments.Select(a => new AppointmentResponse
            {
                AppointmentId = a.Id,
                CustomerName = a.Name,
                Date = a.AppointmentDetail?.TimeWorking.Date ?? DateTime.MinValue,
                Time = a.AppointmentDetail?.TimeWorking.TimeOfDay ?? TimeSpan.Zero,
                EmployeeName = employeeDetails.FirstOrDefault(e => e.Id == a.AppointmentDetail.EmployeeId)?.FullName ?? "",
                TotalPrice = appointmentServiceDetails
                     .Where(asd => asd.AppointmentDetailId == a.AppointmentDetail.Id
                                  && asd.PetName == a.AppointmentDetail.PetName
                                  && asd.PetWeight == a.AppointmentDetail.PetWeight)
                     .Sum(asd => asd.Service.Prices.FirstOrDefault()?.PriceValue ?? 0.0)
                 +
                 comboDetails
                     .Where(c => c.Id == a.AppointmentDetail.ComboId)
                     .SelectMany(c => c.ComboServices
                                       .Where(cs => cs.PetName == a.AppointmentDetail.PetName
                                                 && cs.PetWeight == a.AppointmentDetail.PetWeight))
                     .Sum(cs => cs.Price),
                AppointmentStatus = a.Status
            }).ToList();
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
                appointment.Status = appointmentdto.StatusString;
            }
            await _dbcontext.SaveChangesAsync();
            var appointmentDetail = await _dbcontext.AppointmentDetails
                                                    .FirstOrDefaultAsync(e => e.AppointmentId == appointmentId);
            if (appointmentDetail != null) 
            {
                var newEmployee = await _employeeRepository.GetEmployeeById((int)appointmentdto.AppointmentDetail.EmployeeId);
                var newCombo = await _comboRepository.GetComboById((int)appointmentdto.AppointmentDetail.comboId);
                appointmentDetail.PetName = appointmentdto.AppointmentDetail.PetName;
                appointmentDetail.PetWeight = appointmentdto.AppointmentDetail.PetWeight;
                appointmentDetail.EmployeeId = newEmployee.Id;
                appointmentDetail.Employee = newEmployee;
                appointmentDetail.ComboId = appointmentDetail.ComboId;
                appointmentDetail.Combo = newCombo;
                appointmentDetail.TimeWorking = appointmentdto.AppointmentDetail.TimeWorking;
            }
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<AppointmentDetailResponse> ViewAppointmentDetail(int appointmentId)
        {
            var appointmentDetail = await _dbcontext.AppointmentDetails.AsNoTracking()
                                                                        .Where(e => e.AppointmentId == appointmentId)
                                                                        .Include(e => e.Appointment)
                                                                        .Include(e => e.Employee)
                                                                        .Include(e => e.Combo)
                                                                        .FirstOrDefaultAsync();
            var appointmentServices = await _dbcontext.AppointmentServices
                                                        .AsNoTracking()
                                                        .Include(es => es.Service) // Bao gồm thông tin dịch vụ
                                                        .Where(es => appointmentDetail.Id == es.AppointmentDetailId && es.PetName == appointmentDetail.PetName && es.PetWeight == appointmentDetail.PetWeight)
                                                        .ToListAsync();
            var comboServices = await _dbcontext.ComboServices.AsNoTracking()
                                                              .Where(cs => appointmentDetail.ComboId == cs.ComboId)
                                                              .FirstOrDefaultAsync();

            return new AppointmentDetailResponse
            {
                AppointmentId = appointmentDetail.Appointment.Id,
                AppointmentDetailId = appointmentDetail.Id,
                CustomerName = appointmentDetail.Appointment.Name,
                CustomerEmail = appointmentDetail.Appointment.Email,
                CustomerPhone = appointmentDetail.Appointment.PhoneNumber,
                CustomerAddress = appointmentDetail.Appointment.Address,
                Time = appointmentDetail.TimeWorking.TimeOfDay,
                Date = appointmentDetail.TimeWorking.Date,
                AppointmentStatus = appointmentDetail.Appointment.Status!,
                EmployeeName = appointmentDetail.Employee! != null ? appointmentDetail.Employee!.FullName : "",
                PetName = appointmentDetail.PetName,
                PetWeight = appointmentDetail.PetWeight,
                ComboName = comboServices != null && comboServices.Combo != null ? comboServices.Combo.Name : "",
                Services = appointmentServices != null ? appointmentServices.Select(s => new ServicesResponse
                {
                    ServiceName = s.Service.ServiceName,
                    Price = s.Price
                }).ToList() : new List<ServicesResponse>(),
                BookingTime = (DateTime)appointmentDetail.Appointment.CreatedDate,
                TotalPrice = (appointmentServices != null ? appointmentServices.Sum(e => e.Price) : 0.0) + (comboServices != null ? comboServices.Price : 0.0),
            };
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
                Status = appointmentdto.StatusString,
                IsConfirm = false
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
            appointment.Status = status;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppointmentDetailResponse>> GetAppointmentsByDate(DateTime date)
        {
            var appointmentDetails = await _dbcontext.AppointmentDetails.Where(e => e.TimeWorking ==  date).Select(e => new AppointmentDetailResponse
            {
                AppointmentId = (int)e.AppointmentId!,
                AppointmentStatus = e.Appointment!.Status,
                Date = e.TimeWorking.Date,
                Time = e.TimeWorking.TimeOfDay,
                EmployeeId = (int)e.EmployeeId!,
                EmployeeName = e.Employee!.FullName!
            }).ToListAsync();
            return appointmentDetails;
        }

        public async Task ConfirmAppointment(Appointment appointment)
        {
            appointment.IsConfirm = true;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppointmentResponse>> GetCustomerHistoryAppointments(int customerId, int page, int size)
        {
            
            var appointments = await _dbcontext.Appointments.AsNoTracking()
                                               .Where(a => a.CustomerId == customerId)
                                               .Include(a => a.Customer).Include(a => a.AppointmentDetail)
                                               .Skip((page - 1) * size)
                                               .Take(size).ToListAsync();

            var employeeDetails = await _dbcontext.Employees.AsNoTracking()
                                                            .Where(e => appointments.Select(a => a.AppointmentDetail.EmployeeId).Contains(e.Id))
                                                            .ToListAsync();

            var appointmentServiceDetails = await _dbcontext.AppointmentServices.AsNoTracking()
                                                                               .Include(es => es.Service).ThenInclude(s => s.Prices)
                                                                               .Where(es => appointments.Select(a => a.AppointmentDetail.Id).Contains((int)es.AppointmentDetailId))
                                                                               .ToListAsync();
            var comboDetails = await _dbcontext.Combos.AsNoTracking()
                                                      .Include(c => c.ComboServices)
                                                      .ThenInclude(cs => cs.Service)
                                                      .Where(c => appointments.Select(a => a.AppointmentDetail.ComboId).Contains(c.Id))
                                                      .ToListAsync();
            var result = appointments.Select(a => new AppointmentResponse
            {
                AppointmentId = a.Id,
                CustomerName = a.Customer?.FullName ?? "",
                Date = a.AppointmentDetail?.TimeWorking.Date ?? DateTime.MinValue,
                Time = a.AppointmentDetail?.TimeWorking.TimeOfDay ?? TimeSpan.Zero,
                EmployeeName = employeeDetails.FirstOrDefault(e => e.Id == a.AppointmentDetail.EmployeeId)?.FullName ?? "",
                TotalPrice = appointmentServiceDetails
                     .Where(asd => asd.AppointmentDetailId == a.AppointmentDetail.Id
                                  && asd.PetName == a.AppointmentDetail.PetName
                                  && asd.PetWeight == a.AppointmentDetail.PetWeight)
                     .Sum(asd => asd.Service.Prices.FirstOrDefault()?.PriceValue ?? 0.0)
                 +
                 comboDetails
                     .Where(c => c.Id == a.AppointmentDetail.ComboId)
                     .SelectMany(c => c.ComboServices
                                       .Where(cs => cs.PetName == a.AppointmentDetail.PetName
                                                 && cs.PetWeight == a.AppointmentDetail.PetWeight))
                     .Sum(cs => cs.Price),
                AppointmentStatus = a.Status
            }).ToList();
            return result;
        }
    }
}
