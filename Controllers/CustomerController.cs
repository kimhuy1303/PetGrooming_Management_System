using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public CustomerController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> GetAllCustomers(int page=1, int size=10) {
            var customers = await _userRepository.GetAll(page,size);
            if (customers.IsNullOrEmpty()) return BadRequest("Customers are null or empty!");
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerById(int id)
        {
            var customer = await _userRepository.GetUserById(id);
            if (customer == null) return NotFound("User does not exist!");
            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInfoCustomer(int id, [FromForm] ProfileRequest profiledto)
        {
            if (profiledto == null) return BadRequest(ModelState);
            var customer = await _userRepository.GetUserById(id);
            if (customer == null) return NotFound();
            customer = await _userRepository.ModifyUser(id, profiledto);
            return Ok(new {message = "Updating customer info successfully!",  customer = customer});
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> SearchCustomers(string key)
        {
            var customers = await _userRepository.SearchUser(key);
            if (customers.IsNullOrEmpty()) return NotFound();
            return Ok(customers);
        }
    }
}
