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
        Task<EmployeeDetailsDto?> GetEmployeesByIdAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(string search);
        Task<int> CreateEmployeeAsync(EmployeeViewModel EmployeeVM);
        Task<int> UpdateEmployeeAsync(UpdatedEmployeeDto EmployeeDto);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
