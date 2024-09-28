using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.DAL.Entities.Departments;
using LinkDev.IKEA.DAL.Persistance.Repositories.Departments;
using LinkDev.IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.IKEA.BLL.Services.Departments
{
    public class DepartmentServices(IUnitOfWork _unitOfWork) : IDepartmentServices
    {
       
        public IEnumerable<DepartmentDto> GetDepartments(string search)
        {
            
            var departments = _unitOfWork.DepartmentRepository.GetIQueryable().Where(D => !D.IsDeleted && (string.IsNullOrEmpty(search) || (D.Name.ToLower().Contains(search.ToLower())))).Select(de => new DepartmentDto
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
            var department = _unitOfWork.DepartmentRepository.Get(id);
            if (department is { IsDeleted: false })
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
             _unitOfWork.DepartmentRepository.Add(department);
            return _unitOfWork.Complete();
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
             _unitOfWork.DepartmentRepository.Update(department);
            return _unitOfWork.Complete();
        }
        public bool DeleteDepartment(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department = departmentRepo.Get(id);
            if (department is null)
                return false;
            departmentRepo.Delete(department);
            return _unitOfWork.Complete() > 0;
        }

    }
}
