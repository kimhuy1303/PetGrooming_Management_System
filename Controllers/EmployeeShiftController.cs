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

        [HttpGet("Shifts")]
        [Authorize(Roles = "Employee")]
        public async Task<ICollection<Shift>> GetAllShifts()
        {
            return await _shiftRepository.GetAllShifts();
        }

        [HttpGet("Shift/{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<Shift>> GetShift(int id) 
        {
            var shift = await _shiftRepository.GetShiftById(id);
            if(shift == null) return NotFound("Shift does not exist!");
            return Ok(shift);
        }

        [HttpPost("RegisterShift")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult> RegisterShift([FromBody] EmployeeShiftRequest registerShiftdto)
        {
            var employee =  await _employeeRepository.GetEmployeeById(registerShiftdto.IdEmployee);
            var shift = await _shiftRepository.GetShiftById(registerShiftdto.IdShift);
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

        [HttpDelete()]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult> DeleteShift([FromBody] EmployeeShiftRequest employeeShiftRequest)
        {
            var result = await _employeeShiftRepository.DeleteEmployeeShift(employeeShiftRequest);
            if (result != true) return BadRequest("Deleting failed!");
            return Ok("Deleting succesful");
        }
    }
}
