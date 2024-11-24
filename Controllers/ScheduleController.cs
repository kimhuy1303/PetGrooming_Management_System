using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;
using PetGrooming_Management_System.Utils;
using System.Security.Cryptography;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IEmployeeShiftRepository _employeeShiftRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INotificationRepository _notificationRepository;
        public ScheduleController(IScheduleRepository scheduleRepository, IEmployeeShiftRepository employeeShiftRepository, IEmployeeRepository employeeRepository, INotificationRepository notificationRepository)
        {
            _scheduleRepository = scheduleRepository;
            _employeeShiftRepository = employeeShiftRepository;
            _employeeRepository = employeeRepository;
            _notificationRepository = notificationRepository;
        }

        [HttpGet("RawSchedule")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> AutoSchedule(DateTime start, DateTime end)
        {
            if (new ValidateDateTime().DayRange(start, end) != 5) return BadRequest("Date is not valid for scheduling!");
            if (start.Date < DateTime.UtcNow && end.Date < DateTime.UtcNow) return BadRequest("Date is overdue for scheduling");
            //var amountEmployeeShiftsAssigned = await _employeeShiftRepository.GetNumberOfEmployeeRegisterShiftForAWeek(start, end);
            //if (amountEmployeeShiftsAssigned < await _employeeRepository.CountEmployee()) return BadRequest("The number of registered employees is not enough!");
            var raw = await _scheduleRepository.RawSchedule(start, end); 
            return Ok(raw);
        }
        [HttpPost("ConfirmSchedule")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> ConfirmSchedule([FromBody] ScheduleRequest rawSchedule)
        {
            if (rawSchedule == null) return BadRequest(ModelState);
            await _scheduleRepository.CreateSchedule(rawSchedule);
            var message = "The work schedule (" + rawSchedule.StartDate.ToString("dd/MM/yyyy")+"-"+rawSchedule.EndDate.ToString("dd/MM/yyyy") + " has been announced. Please check your work schedule!";
            await _notificationRepository.SendNotificationToAllEmployees(message);
            return Ok("Scheduling automation is successful!");
        }

        [HttpGet("Employee/{id}")]
        public async Task<ActionResult> GetEmployeeShiftsByEmployeeId(int id, DateTime start, DateTime end)
        {
            if (new ValidateDateTime().DayRange(start, end) != 5) return BadRequest("Date is not valid!");
            var employee = await _employeeRepository.GetEmployeeById(id);
            if (employee == null) return BadRequest("Employee does not exist!");
            var schedule = await _scheduleRepository.GetScheduleByWeek(start, end);
            if (schedule == null) return BadRequest("Schedule deos not exist!");
            IEnumerable<object> res = await _scheduleRepository.GetEmployeeShiftsByEmployee(id, schedule);
            return Ok(res);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> GetScheduleByWeek(DateTime start, DateTime end)
        {
            var schedule = await _scheduleRepository.GetScheduleByWeek(start, end);
            if (schedule == null) return BadRequest("This schedule does not exist!");
            return Ok(schedule);
        }

        [HttpPut("UpdateShift/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> UpdateEmployeeShiftInSchedule(int id, [FromBody] EmployeeShiftRequest employeeShiftRequest)
        {
            if (employeeShiftRequest == null) return BadRequest(ModelState);
            var schedule = await _scheduleRepository.GetScheduleById(id);
            if (schedule == null) return NotFound("This schedule does not exist!");
            var existingEmployeeShift = await _scheduleRepository.GetEmployeeShiftInSchedule(id, employeeShiftRequest);
            if(existingEmployeeShift != null)
            {
                // remove ca làm cũ 
                await _scheduleRepository.RemoveEmployeeShift(id, employeeShiftRequest);
            }
            // Update ca làm mới
            await _scheduleRepository.UpdateEmloyeeShiftInSchedule(id, employeeShiftRequest);
            return Ok("Updating schedule successfully!");

        }
    }
}
