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
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IServiceRepository serviceRepository, ILogger<ServiceController> logger, IPriceRepository priceRepository)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
            _priceRepository = priceRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<Service>>> GetAllServices()
        {
            var services = await _serviceRepository.GetAllServices();
            if (services.IsNullOrEmpty()) return BadRequest("Sevices are null or empty");
            return Ok(services);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetServiceById(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service == null) return NotFound();
            return Ok(service);
        }
        [HttpGet("ByPet")]
        public async Task<ActionResult<Service>> GetServicesByPet(string petName, string petWeight)
        {
            if (petName.IsNullOrEmpty()|| petWeight.IsNullOrEmpty()) return BadRequest(ModelState);
            var services = await _serviceRepository.GetServicesByPet(petName, petWeight);
            if (services.IsNullOrEmpty()) return NotFound();
            return Ok(services);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateService([FromBody] ServiceRequest servicedto)
        {
            if (servicedto == null) return BadRequest(ModelState);

            var newService = await _serviceRepository.GetServiceByName(servicedto.ServiceName);

            if (newService != null)
            {
                    return BadRequest("Service already existed!");
            }
            else
            {
                newService = await _serviceRepository.CreateService(servicedto);
                return Ok(new { message = "Service created succesfully", service = newService });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> UpdateService(int id, [FromBody] ServiceRequest servicedto)
        {
            if (servicedto == null) return BadRequest(ModelState);

            var service = await _serviceRepository.GetServiceById(id);
            if (service == null) return BadRequest("Something went wrong in updating service!");
            await _serviceRepository.UpdateService(service, servicedto);
            return Ok(new {message = "Updating service successfully", service = service});
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> DeleteService(int id)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service == null) return NotFound(new {message = "Service does not found!"});
            
            await _serviceRepository.DeleteService(service);
            return Ok("Deleting service successfully");
        }

        [HttpPut("RemovePrice/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> RemovePriceService(int id, PriceRequest pricedto)
        {
            var service = await _serviceRepository.GetServiceById(id);
            if (service == null) return NotFound(new { message = "Service does not found!" });
            var price = await _priceRepository.IsPriceExist(pricedto);
            if (price == null) return NotFound(new { message = "Service does not have this price!" });
            await _serviceRepository.RemovePrice(service, price);
            return Ok(new { message = "Remove price successfully!" });
        }

        
    }
}
