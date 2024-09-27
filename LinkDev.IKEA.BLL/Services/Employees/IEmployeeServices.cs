using LinkDev.IKEA.BLL.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Employees
{
    public interface IEmployeeServices
    {
        EmployeeDetailsDto? GetEmployeesById(int id);
        IEnumerable<EmployeeDto> GetEmployees(string search);
        int CreateEmployee(CreatedEmployeeDto EmployeeDto);
        int UpdateEmployee(UpdatedEmployeeDto EmployeeDto);
        bool DeleteEmployee(int id);
    }
}
