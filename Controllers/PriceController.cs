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
    public class PriceController : ControllerBase
    {
        private readonly IPriceRepository _priceRepository;
        private readonly IServiceRepository _serviceRepository;
        public PriceController(IPriceRepository priceRepository, IServiceRepository serviceRepository)
        {
            _priceRepository = priceRepository;
            _serviceRepository = serviceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Price>>> GetAllPrices(){
            var res = await _priceRepository.GetAllPrices();
            if (res.IsNullOrEmpty()) return BadRequest("List price is empty or null");
            return Ok(res);
        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> AddPrice(int serviceId, [FromBody] PriceRequest pricedto)
        {
            if (pricedto == null) return BadRequest(ModelState);

            var service = await _serviceRepository.GetServiceById(serviceId);
            var price = await _priceRepository.IsPriceExist(pricedto);
            if (service == null) return NotFound();
            if(price == null)
            {
                var newPrice = await _priceRepository.CreatePrice(pricedto);
                await _priceRepository.AddPrice(newPrice.Id, serviceId);
                return Ok(new { message = "Adding price successfully", service = service});
            }
            else
            {
                await _priceRepository.AddPrice(price.Id, serviceId);
                return Ok(new { message = "Adding price successfully", service = service });
            }
            
        }
    }
}
