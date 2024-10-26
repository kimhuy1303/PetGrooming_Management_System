using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly ILogger<ShiftController> _logger;

        public ShiftController(IShiftRepository shiftRepository, ILogger<ShiftController> logger)
        {
            _shiftRepository = shiftRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<ActionResult<ICollection<Shift>>> GetAllShifts()
        {
            var listShifts = await _shiftRepository.GetAllShifts();
            if (listShifts.IsNullOrEmpty()) return BadRequest("List shifts are null or empty");
            return Ok(listShifts);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,Manager")]
        public async Task<ActionResult<Shift>> GetShiftById(int id)
        {
            var shift = await _shiftRepository.GetShiftById(id);
            if (shift == null) return NotFound("Shift does not exist!");
            return Ok(shift);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateShift([FromBody] ShiftRequest shiftdto)
        {
            if (shiftdto == null) return BadRequest(ModelState);

            var isCreate = await _shiftRepository.CreateShift(shiftdto);
            if (isCreate != true) return BadRequest("Something went wrong in creating new shift!");
            return Ok("Creating successfully");    
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> UpdateShift(int id, [FromBody] ShiftRequest shiftdto)
        {
            if (shiftdto == null) return BadRequest(ModelState);

            /* Check Exists
            var shift = await _shiftRepository.GetShiftById(id);
            if(shift == null) return NotFound(ModelState);
            */

            var res = await _shiftRepository.UpdateShift(id, shiftdto);
            if(res != true) return BadRequest("Shift does not exist or something went wrong in updating shift!");
            return Ok("Updating Successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteShift(int id)
        {
            /* Check Exists
            var shift = await _shiftRepository.GetShiftById(id);
            if(shift == null) return NotFound(ModelState);
            */

            var isDelete = await _shiftRepository.DeleteShiftById(id);
            if (isDelete != true) return BadRequest("Shift does not exist or something went wrong in deleting shift!");
            return Ok("Deleting successfully");
        }


    }
}
