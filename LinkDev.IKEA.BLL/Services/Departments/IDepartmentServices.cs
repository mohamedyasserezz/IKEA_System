using LinkDev.IKEA.BLL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Departments
{
    public interface IDepartmentServices
    {
        Task<DepartmentDetailsDto?> GetDepartmentsByIdAsync(int id);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(string search);
        Task<int> CreateDepartmentAsync(CreatedDepartmentDto departmentDto);
        Task<int> UpdateDepartmentAsync(UpdatedDepartmentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
