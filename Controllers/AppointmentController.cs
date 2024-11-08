using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
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
            if(valid.IsValid(DateTime.Now, appointmentdto.AppointmentDetail.TimeWorking) != true) return BadRequest("Datetime is over!");
            if(appointmentdto.AppointmentDetail.comboId != 0 || !appointmentdto.AppointmentDetail.AppointmentServices.IsNullOrEmpty())
            {
                if (await _comboRepository.GetComboById((int)appointmentdto.AppointmentDetail.comboId) == null) return BadRequest("Combo does not exist!");
                foreach(AppointmentServicesRequest service in appointmentdto.AppointmentDetail.AppointmentServices)
                {
                    if (await _serviceRepository.GetServiceById(service.ServiceId) == null) return BadRequest("ServiceId " + service.ServiceId + " does not exist!");
                }
            }
            await _appointmentRepository.MakeAnAppointment(customerId, appointmentdto);
            return Ok("Making an appointmeent successfull!");
        }


    }
}
