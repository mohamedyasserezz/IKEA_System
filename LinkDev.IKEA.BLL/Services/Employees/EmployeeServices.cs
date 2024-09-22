using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistance.Repositories.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    public class EmployeeServices(IEmployeeRepository employeeRepository) : IEmployeeServices
    {
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;

        public IEnumerable<EmployeeDto> GetAllEmployees()
        {
            var employees = _employeeRepository.GetIQueryable()
                .Where(E => !E.IsDeleted).Include(E => E.Department)
                .Select(EmployeeDto => new EmployeeDto
                {
                    Id = EmployeeDto.Id,
                    Name = EmployeeDto.Name,
                    Age = EmployeeDto.Age,
                    Salary = EmployeeDto.Salary,

                    Email = EmployeeDto.Email,
                    IsActive = EmployeeDto.IsActive,
                    Gender = nameof(EmployeeDto.Gender),
                    EmployeeType = nameof(EmployeeDto.EmployeeType),
                    Department = EmployeeDto.Department!.Name,

                }).ToList();

            return employees;
        }

        public EmployeeDetailsDto? GetEmployeesById(int id)
        {
            var employee = _employeeRepository.Get(id);
            if (employee is { IsDeleted: false })
                return new EmployeeDetailsDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Salary = employee.Salary,
                    Address = employee.Address,
                    HiringDate = employee.HiringDate,
                    Department = employee.Department?.Name,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    IsActive = employee.IsActive,
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType,

                };
            return null;

        }

        public int CreateEmployee(CreatedEmployeeDto EmployeeDto)
        {
            var employee = new Employee
            {
                DepartmentId = EmployeeDto.DepartmentId,
                Name = EmployeeDto.Name,
                Age = EmployeeDto.Age,
                Salary = EmployeeDto.Salary,
                Address = EmployeeDto.Address,
                HiringDate = EmployeeDto.HiringDate,
                Email = EmployeeDto.Email,
                PhoneNumber = EmployeeDto.PhoneNumber,
                IsActive = EmployeeDto.IsActive,
                Gender = EmployeeDto.Gender,
                EmployeeType = EmployeeDto.EmployeeType,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };

            return _employeeRepository.Add(employee);
        }
        public int UpdateEmployee(UpdatedEmployeeDto EmployeeDto)
        {
            var employee = new Employee
            {
                Id = EmployeeDto.Id,
                Name = EmployeeDto.Name,
                Age = EmployeeDto.Age,
                Salary = EmployeeDto.Salary,
                Address = EmployeeDto.Address,
                HiringDate = EmployeeDto.HiringDate,
                Email = EmployeeDto.Email,
                PhoneNumber = EmployeeDto.PhoneNumber,
                IsActive = EmployeeDto.IsActive,
                Gender = EmployeeDto.Gender,
                EmployeeType = EmployeeDto.EmployeeType,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
                DepartmentId = EmployeeDto.DepartmentId,
            };

            return _employeeRepository.Update(employee);
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeRepository.Get(id);
            if (employee == null)
                return false;
            return _employeeRepository.Delete(employee) > 0;

        }



    }
}
