using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Models.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly CreationDate { get; set; }

        /// public static explicit operator DepartmentDto(Department department)
        /// {
        ///     return new DepartmentDto
        ///     {
        ///         Id = department.Id,
        ///         Name = department.Name,
        ///         Code = department.Code,
        ///         Description = department.Description,
        ///         CreationDate = department.CreationDate,
        ///     };
        /// }
    }
}
