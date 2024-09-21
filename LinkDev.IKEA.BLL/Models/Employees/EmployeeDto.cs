using LinkDev.IKEA.DAL.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Models.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "Max Length is 50 chars")]
        [MinLength(5, ErrorMessage = "Min Length is 5 chars")]
        public string Name { get; set; } = null!;

        [Range(22, 30)]
        public int? Age { get; set; }

       
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "IS Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
      
        public string Gender { get; set; } = string.Empty;

        [Display(Name = "Employee Type")]
        public string EmployeeType { get; set; } = string.Empty;
    }
}
