using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterShiftController : ControllerBase
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly MainDBContext _dbContext;

        
        public RegisterShiftController(MainDBContext mainDBContext, IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
        {
            _dbContext = mainDBContext;
            _shiftRepository = shiftRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ICollection<Shift>> GetAllShifts()
        {
            return await _shiftRepository.GetAllShifts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShift(int id) 
        {
            var shift = await _shiftRepository.GetShiftById(id);
            if(shift == null) return NotFound("Shift does not exist!");
            return Ok(shift);
        }

        [HttpPost("Employee")]
        public async Task<ActionResult> RegisterShift([FromBody] RegisterShiftRequest registerShiftdto)
        {
            var employee = _employeeRepository.GetEmployeeById(registerShiftdto.IdEmployee);
            if (employee == null) return BadRequest("Employee does not exist!");
            await _employeeRepository.RegisterShift(registerShiftdto);
            return Ok("Registering shift successfully");
        }
    }
}
