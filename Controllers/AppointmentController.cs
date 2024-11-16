using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Configs.Constant;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Utils;
using System.Security.Claims;
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
            await _appointmentRepository.ChangeStatusAppointment(appointment, "InProgress");
            var appointmentDetail = await _appointmentRepository.ViewAppointmentDetail(appointment.Id);
            var message = "The customer's appointment has been confirmed by staff. Please check your appointment history to see details!";
            //await _notificationRepository.SendNotificationToCustomer(message, (int)appointment.CustomerId, appointment.Id);
            return Ok("Appointment confirmed from staff!");
        }

        [HttpPut("Edit/{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> EditAppointment(int id, [FromForm] AppointmentRequest appointmentdto)
        {
            if (appointmentdto == null) return BadRequest(ModelState);
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null) return BadRequest("Appointment does not exist!");
            await _appointmentRepository.UpdateAppointment(id, appointmentdto);
            return Ok("Edit appointment successfully!");
        }

        [HttpPut("UpdateStatus/{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> UpdateAppointmentStatus(int id, string statusString)
        {
            if (string.IsNullOrEmpty(statusString)) return BadRequest("Status field is required!");
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment == null) return BadRequest("Appointment does not exist!");
            await _appointmentRepository.ChangeStatusAppointment(appointment, statusString);
            var message = "Your appointment (ID: " + appointment.Id + ") is " + statusString;
            //await _notificationRepository.SendNotificationToCustomer(message, (int)appointment.CustomerId, appointment.Id);
            return Ok("Update status successfully!");
        }

        [HttpPut("RequestCancel/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> RequestCancel(int id)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            var userIdFromToken = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (appointment.CustomerId.ToString() != userIdFromToken) return Forbid("You do not have permission to request cancel this appointment.");
            if (appointment == null) return NotFound(new { message = "Appointment not found" });
            if (appointment.Status == Status.Canceled || appointment.Status == Status.PendingCancellation)
            {
                return BadRequest(new { message = "Appointment is already canceled or pending cancellation" });
            }

            await _appointmentRepository.ChangeStatusAppointment(appointment, "PendingCancellation");
            return Ok(new { message = "Cancellation request submitted. Waiting for confirmation." });
        }

        [HttpPut("ConfirmCancel/{id}")]
        [Authorize(Roles = "Employee, Manager")]
        public async Task<ActionResult> ConfirmCancel(int id, bool isApprove)
        {
            var appointment = await _appointmentRepository.GetAppointmentById(id);
            if (appointment.Status != Status.PendingCancellation) return BadRequest(new { message = "Appointment is not pending cancellation" });
            if (isApprove)
            {
                await _appointmentRepository.ChangeStatusAppointment(appointment, "Canceled");
            }
            else
            {
                await _appointmentRepository.ChangeStatusAppointment(appointment, "InProgress");
            }
            return Ok(new { message = isApprove ? "Appointment canceled." : "Cancellation request denied." });
            }
    }
}