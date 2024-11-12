using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeShiftController : ControllerBase
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeShiftRepository _employeeShiftRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly MainDBContext _dbContext;

        
        public EmployeeShiftController(MainDBContext mainDBContext, IShiftRepository shiftRepository, IEmployeeShiftRepository employeeShiftRepository, IEmployeeRepository employeeRepository)
        {
            _dbContext = mainDBContext;
            _shiftRepository = shiftRepository;
            _employeeShiftRepository = employeeShiftRepository;
            _employeeRepository = employeeRepository;
        }

        

        [HttpPost("RegisterShift")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult> RegisterShift([FromBody] EmployeeShiftRequest registerShiftdto)
        {
            if(registerShiftdto == null) return BadRequest(ModelState);
            var employee =  await _employeeRepository.GetEmployeeById(registerShiftdto.EmployeeId);
            var shift = await _shiftRepository.GetShiftById(registerShiftdto.ShiftId);
            if (employee == null) return BadRequest("Employee does not exist!");
            if (shift == null) return BadRequest("Shift does not exist!");
            var res = await _employeeShiftRepository.RegisterShift(registerShiftdto);
            if(res != true) return BadRequest("Date is not valid");
            return Ok("Registering shift successfully");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<ActionResult<ICollection<EmployeeShift>>> GetEmployeeShifts(int id)
        {
            var result = await _employeeShiftRepository.GetEmployeeShifts(id);
            if(result.IsNullOrEmpty()) return BadRequest("Employee does not exist or does not register shifts!");
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> EditEmployeeShifts(int id, [FromBody] EmployeeShiftRequest editdto)
        {
            if (editdto == null) return BadRequest(ModelState);

            if (id != editdto.EmployeeId) return BadRequest("EmployeeId does not exist or does not register shift");
            var isExist = await _employeeShiftRepository.GetEmployeeShift(editdto.EmployeeId, editdto.Date);
            if(isExist != null)
            {
                // remove shift cũ
                await _employeeShiftRepository.DeleteEmployeeShift(editdto);
            }
            await _employeeShiftRepository.UpdateEmployeeShift(editdto);
            return Ok("Updating shifts successfully!");
        }

        [HttpGet("GetShiftsForWeek")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetAllEmployeeShiftsForWeek(DateTime start, DateTime end)
        {
            var res = await _employeeShiftRepository.GetEmployeeShiftsForWeek(start, end);
            if(res.IsNullOrEmpty()) return BadRequest("This week does not have any shifts");
            return Ok(res);
        }
        [HttpGet("GetShiftsForDate")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetAllEmployeeShiftsForDate(DateTime date)
        {
            var res = await _employeeShiftRepository.GetEmployeeShiftsByDay(date);
            if (res.IsNullOrEmpty()) return BadRequest("This date does not have any shifts");
            return Ok(res);
        }
    }
}
