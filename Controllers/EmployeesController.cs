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
        private readonly IEmployeeShiftRepository _employeeShiftRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeShiftRepository employeeShiftRepository, IAppointmentRepository appointmentRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeShiftRepository = employeeShiftRepository;
            _appointmentRepository = appointmentRepository;
        }

        // GET: api/Employees
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ICollection<Employee>>> GetEmployees(int page=1, int size=10)
        {
            var listEmployees = await _employeeRepository.GetEmployeesPaged(page,size);
            if (listEmployees.IsNullOrEmpty()) return BadRequest("List employees are null or empty!");
            return Ok(listEmployees);
        }

        [HttpGet("AllEmployees")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ICollection<Employee>>> GetEmployees()
        {
            var listEmployees = await _employeeRepository.GetAllEmployees();
            if (listEmployees.IsNullOrEmpty()) return BadRequest("List employees are null or empty!");
            return Ok(listEmployees);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Employee,Manager")]
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
        [Authorize(Roles = "Manager, Employee")]
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

        [HttpGet("EmployeesPerSlotByDate")]
        [Authorize(Roles ="Manager, Employee")]
        public async Task<ActionResult> GetEmployeesPerTimeSlotInDate(DateTime date, int timeslot)
        {
            var employeeShift = await _employeeShiftRepository.GetEmployeeShiftsByDay(date);
            if (employeeShift == null) return NotFound(new {message = "This date has no employee working!" });
            var employeesInTimeslot = employeeShift.Where(e => e.Shift.TimeSlot  == timeslot).Select(e => new
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.Employee.FullName,
                ShiftId = e.ShiftId,
                TimeSlot = e.Shift.TimeSlot,
                Date = e.Date
            });
            if (employeesInTimeslot == null) return NotFound(new { message = "This timeslot has no employee working!" });
            return Ok(employeesInTimeslot);
        }
        [HttpGet("EmployeesWorkingInTimeByDate")]
        [Authorize(Roles = "Manager, Employee")]
        public async Task<ActionResult> GetEmployeesWorkingInTimeByDate(DateTime date, TimeOnly time)
        {
            var employeeShift = await _employeeShiftRepository.GetEmployeeShiftsByDay(date);
            if (employeeShift == null) return NotFound(new { message = "This date has no employee working!" });
            var appointmentDetails = await _appointmentRepository.GetAppointmentsByDate(date);

            var employeesWorkingInTime = employeeShift.Where(e => e.Shift.StartTime <= time && e.Shift.EndTime > time)
                                                      .Where(e => !appointmentDetails.Any(ad => ad.AppointmentStatus != "Canceled" &&ad.EmployeeId == e.EmployeeId &&
                                                                                          TimeOnly.FromTimeSpan(ad.Time) >= e.Shift.StartTime &&
                                                                                          TimeOnly.FromTimeSpan(ad.Time) < e.Shift.EndTime))
                                                      .Select(e => new
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.Employee?.FullName,
                ShiftId = e.ShiftId,
                TimeSlot = e.Shift.TimeSlot,
                Time = time,
                StartTime = e.Shift?.StartTime,
                EndTime = e.Shift?.EndTime,
                Date = e.Date
            }).ToList();
            if (employeesWorkingInTime == null || !employeesWorkingInTime.Any()) return NotFound(new { message = "No employees available in this time slot!" });
            return Ok(employeesWorkingInTime);
        }
    }
}
