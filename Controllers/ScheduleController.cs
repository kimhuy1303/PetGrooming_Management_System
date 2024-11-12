using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
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
        public ScheduleController(IScheduleRepository scheduleRepository, IEmployeeShiftRepository employeeShiftRepository, IEmployeeRepository employeeRepository)
        {
            _scheduleRepository = scheduleRepository;
            _employeeShiftRepository = employeeShiftRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpPost("CreateSchedule")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> AutoSchedule(DateTime start, DateTime end)
        {
            if (new ValidateDateTime().DayRange(start, end) != 5) return BadRequest("Date is not valid for scheduling!");
            var amountEmployeeShiftsAssigned = await _employeeShiftRepository.GetNumberOfEmployeeRegisterShiftForAWeek(start, end);
            if (amountEmployeeShiftsAssigned < await _employeeRepository.CountEmployee()) return BadRequest("The number of registered employees is not enough!");
            await _scheduleRepository.CreateSchedule(start, end);
            return Ok("Scheduling automation is successful!");

        }

        [HttpGet("/Employee/{id}")]
        public async Task<ActionResult> GetEmployeeShiftsByEmployeeId(int employeeId, DateTime start, DateTime end)
        {
            if (new ValidateDateTime().DayRange(start, end) != 5) return BadRequest("Date is not valid!");
            var employee = await _employeeRepository.GetEmployeeById(employeeId);
            if (employee == null) return BadRequest("Employee does not exist!");
            var schedule = await _scheduleRepository.GetScheduleByWeek(start, end);
            if (schedule == null) return BadRequest("Schedule deos not exist!");
            var res = await _scheduleRepository.GetEmployeeShiftsByEmployee(employeeId, schedule.startDate, schedule.endDate);
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

        [HttpPut("update-shift/{scheduleId}")]
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
