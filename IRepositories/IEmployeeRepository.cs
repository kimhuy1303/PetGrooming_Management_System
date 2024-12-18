﻿using PetGrooming_Management_System.DTOs.Requests;
using PetGrooming_Management_System.Models;

namespace PetGrooming_Management_System.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployeeById(int id);
        Task<Employee> GetEmployeeByIdenNumber(string idenNumber);
        Task<ICollection<Employee>> GetAllEmployees();
        Task<ICollection<Employee>> GetEmployeesPaged(int page, int size);
        Task AddEmployee(EmployeeProfileRequest employeeDTO);
        Task DeleteEmployee(int id);    
        Task ModifyProfileEmployee(int id, EmployeeProfileRequest profile);

        Task<int> CountEmployee();
        
    }
}
