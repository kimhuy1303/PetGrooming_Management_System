using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Utils;
namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IComboRepository _comboRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmployeeShiftRepository _employeeShiftRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository, IServiceRepository serviceRepository, IComboRepository comboRepository, INotificationRepository notificationRepository, IEmployeeShiftRepository employeeShiftRepository)
        {

            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
            _comboRepository = comboRepository;
            _notificationRepository = notificationRepository;
            _employeeShiftRepository = employeeShiftRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> GetAllAppointments()
        {
            var result = await _appointmentRepository.GetAllAppointments();
            if (result.IsNullOrEmpty()) return BadRequest("Appointments are null or empty!");
            return Ok(result);
        }

        [HttpGet("ViewAppointmentDetail/{id}")]
        public async Task<ActionResult> ViewAppointmentDetail(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null) return NotFound("Appointment does not exist!");
            var detail = await _appointmentRepository.ViewAppointmentDetail(appointment.Id);
            return Ok(detail);
        }

        [HttpPost("MakeAppointment")]
        public async Task<ActionResult> MakeAnAppointment(int customerId, [FromForm] AppointmentRequest appointmentdto)
        {
            var valid = new ValidateDateTime();
            if (appointmentdto == null) return BadRequest(ModelState);
            if (!(await _employeeShiftRepository.IsEmployeeWorkingByDateInSchedule(appointmentdto.AppointmentDetail.EmployeeId, appointmentdto.AppointmentDetail.TimeWorking))) return BadRequest("This groomer does not work in this day!");
            
            if(valid.IsValid(DateTime.Now, appointmentdto.AppointmentDetail.TimeWorking) != true) return BadRequest("Datetime is overdue!");
            if (appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty() && appointmentdto.AppointmentDetail.comboId == 0) return BadRequest("Need to choose at least one service or combo");
            if(appointmentdto.AppointmentDetail.comboId != 0 )
            {
                if (await _comboRepository.GetComboById((int)appointmentdto.AppointmentDetail.comboId) == null) return BadRequest("Combo does not exist!");
            }
            if (!appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty())
            {
                foreach(AppointmentServicesRequest service in appointmentdto.AppointmentDetail.AppointmentServices)
                {
                    if (await _serviceRepository.GetServiceById(service.ServiceId) == null) return BadRequest("ServiceId " + service.ServiceId + " does not exist!");
                }
            }
            var appointmentDetail = await _appointmentRepository.MakeAnAppointment(customerId, appointmentdto);
            var message = "There is a new appointment that needs to be confirmed (ID: " + appointmentDetail.AppointmentId + ")";
            //await _notificationRepository.SendNotificationToEmployee(message, (int)appointmentDetail.EmployeeId, (int)appointmentDetail.AppointmentId);
            return Ok("Making an appointmeent successfully!");
        }

        [HttpPost("Confirm")]
        public async Task<ActionResult> ConfirmAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(appointmentId);
            if (appointment == null) return NotFound();
            await _appointmentRepository.ChangeStatusAppointment(appointment, Configs.Constant.Status.InProgress);
            var appointmentDetail = await _appointmentRepository.ViewAppointmentDetail(appointment.Id);
            var message = "The customer's appointment has been confirmed by staff. Please check your appointment history to see details!";
            //await _notificationRepository.SendNotificationToCustomer(message, (int)appointment.CustomerId, appointment.Id);
            return Ok("Appointment confirmed from staff!");
        }

    }
}