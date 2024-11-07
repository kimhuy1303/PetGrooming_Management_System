using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetGrooming_Management_System.Data;
using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.IRepositories;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: api/Employees
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ICollection<Employee>>> GetEmployees()
        {
            var listEmployees = await _employeeRepository.GetAllEmployees();
            if (listEmployees.IsNullOrEmpty()) return BadRequest("List employees are null or empty!");
            return Ok(listEmployees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound("Employee does not found!");
            }

            return Ok(employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromForm] EmployeeProfileRequest employee)
        {
            var _employee = await _employeeRepository.GetEmployeeById(id);
            if ( _employee == null)
            {
                return BadRequest("Employee does not exist to modify!");
            }
            await _employeeRepository.ModifyProfileEmployee(id, employee);
            return Ok("Modifying employee successfully!");
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<Employee>> PostEmployee([FromForm] EmployeeProfileRequest employee)
        {
            var _employee = await _employeeRepository.GetEmployeeByIdenNumber(employee.IdentificationNumber!);
            if (_employee != null) return BadRequest("Employee has existed");
            await _employeeRepository.AddEmployee(employee);
            return Ok("Adding employee successfully");
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeRepository.DeleteEmployee(id);
            return Ok("Deleting employee successfully");
        }

    }
}
