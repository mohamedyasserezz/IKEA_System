using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistance.Repositories.Employees;
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
            return _employeeRepository.GetAllAsIQueryable().Select(
               EmployeeDto => new EmployeeDto
               {
                   Id = EmployeeDto.Id,
                   Name = EmployeeDto.Name,
                   Age = EmployeeDto.Age,
                   Salary = EmployeeDto.Salary,

                   Email = EmployeeDto.Email,
                   IsActive = EmployeeDto.IsActive,
                   Gender = nameof(EmployeeDto.Gender),
                   EmployeeType = nameof(EmployeeDto.EmployeeType),

               });
        }

        public EmployeeDetailsDto? GetEmployeesById(int id)
        {
            var employee = _employeeRepository.Get(id);
            if (employee == null)
                return null;
            return new EmployeeDetailsDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                Address = employee.Address,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                Gender = nameof(employee.Gender),
                EmployeeType = nameof(employee.EmployeeType),

            };
        }

        public int CreateEmployee(CreatedEmployeeDto EmployeeDto)
        {
            var employee = new Employee
            {
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
