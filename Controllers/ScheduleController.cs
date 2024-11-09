using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ScheduleController(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        [HttpPost("CreateSchedule")]
        public async Task<ActionResult> AutoSchedule(DateTime start, DateTime end)
        {
            if (new ValidateDateTime().DayRange(start, end) != 5) return BadRequest("Date is not valid for scheduling!");
            await _scheduleRepository.CreateSchedule(start, end);
            return Ok("Scheduling automation is successful!");

        }

        [HttpGet]
        public async Task<ActionResult> GetScheduleByWeek(DateTime start, DateTime end)
        {
            var schedule = await _scheduleRepository.GetScheduleByWeek(start, end);
            if (schedule == null) return BadRequest("This schedule does not exist!");
            return Ok(schedule);
        }
    }
}
