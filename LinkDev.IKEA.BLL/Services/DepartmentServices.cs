using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.DAL.Entities.Department;
using LinkDev.IKEA.DAL.Persistance.Repositories.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services
{
    public class DepartmentServices(IDepartmentRepository repository) : IDepartmentServices
    {
        private readonly IDepartmentRepository _repository = repository;
        public IEnumerable<DepartmentDto> GetAllDepartments()
        {
            var departments = _repository.GetAllAsIQueryable().Select(de => new DepartmentDto
            {
                Id = de.Id,
                Code = de.Code,
                Name = de.Name,
                CreationDate = de.CreationDate,
            }).AsNoTracking().ToList();
            return departments;

            /// foreach (var department in departments)
            ///     yield return new DepartmentDto
            ///     {
            ///         Id = department.Id,
            ///         Code = department.Code,
            ///         Name = department.Name,
            ///         CreationDate = department.CreationDate,
            ///         Description = department.Description,
            ///     };
        }

        public DepartmentDetailsDto? GetDepartmentsById(int id)
        {
            var department = _repository.Get(id);
            if (department is { })
                return new DepartmentDetailsDto
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    Description = department.Description,
                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModifiedBy = department.LastModifiedBy,
                    LastModifiedOn = department.LastModifiedOn,
                };
            return null;
        }

        public int CreateDepartment(CreatedDepartmentDto departmentDto)
        {
            var department = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                CreationDate = departmentDto.CreationDate,
                Description = departmentDto.Description,
                CreatedBy = 1,
                //CreatedOn = DateTime.UtcNow,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
            return _repository.Add(department);
        }

        public int UpdateDepartment(UpdatedDepartmentDto departmentDto)
        {
            var department = new Department
            {
                Id = departmentDto.Id,
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,

            };
            return _repository.Update(department);
        }
        public bool DeleteDepartment(int id)
        {
            var department = _repository.Get(id);
            if (department is null)
                return false;
            _repository.Delete(department);
            return true;
        }




    }
}
