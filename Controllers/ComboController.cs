using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.DTOs.Respones;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IComboRepository _comboRepository;
        public ComboController( IComboRepository comboRepository)
        {
            _comboRepository = comboRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCombos()
        {
            var combos = await _comboRepository.GetAllCombos();
            if (combos.IsNullOrEmpty()) return BadRequest("Combos are empty or null!");
            return Ok(combos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetComboById(int id)
        {
            var combo = await _comboRepository.DisplayComboById(id);
            if (combo == null) return NotFound();
            return Ok(combo);
        }

        [HttpPost("Create-combo")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Combo>> CreateCombo([FromBody] ComboRequest combodto)
        {
            if (combodto == null) return BadRequest(ModelState);
            var newCombo = await _comboRepository.GetComboByName(combodto.ComboName);
            if (newCombo != null) return BadRequest("Combo already existed!");
            newCombo = await _comboRepository.CreateCombo(combodto);
            return Ok(new { message = "Creating combo succesfully", combo =  newCombo });
        }

        [HttpPost("Add-services")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Combo>> AddServicesToCombo([FromBody] ComboServiceRequest comboServicedto)
        {
            if (comboServicedto == null) return BadRequest(ModelState);
            var combo = await _comboRepository.GetComboById(comboServicedto.ComboId);
            if (combo == null) return NotFound();
            combo = await _comboRepository.AddServicesToCombo(comboServicedto);
            return Ok(new { message = "Adding services to combo successfully", combo = combo });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Combo>> UpdateCombo(int id, [FromBody] ComboRequest combodto)
        {
            var combo = await _comboRepository.GetComboById(id);
            if(combo == null) return NotFound();
            var comboUpdated = await _comboRepository.UpdateCombo(combo, combodto);
            return Ok(new { message = "Updating combo successfully", combo = comboUpdated });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteCombo(int id)
        {
            var combo = await _comboRepository.GetComboById(id);
            if (combo == null) return NotFound();
            await _comboRepository.DeleteCombo(combo.Id);
            return Ok("Deleting combo " + combo.Id + " successfully");
        }


        [HttpDelete("Removing-Service")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> RemoveServicesFromCombo(int comboId, int serviceId)
        {
             var isRemove = await _comboRepository.RemoveServicesFromCombo(comboId, serviceId);
            if(isRemove) return Ok("Remove serviceId " + serviceId + " from comboId " + comboId + " successfully!");
            return BadRequest("Something went wrong in removing service from combo!"); 
        }
    }
}
