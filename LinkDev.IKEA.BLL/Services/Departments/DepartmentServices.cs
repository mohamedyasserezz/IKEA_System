using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.DAL.Entities.Departments;
using LinkDev.IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.IKEA.BLL.Services.Departments
{
    public class DepartmentServices(IUnitOfWork _unitOfWork) : IDepartmentServices
    {
       
        public async Task<IEnumerable<DepartmentDto>> GetDepartmentsAsync(string search)
        {
            
            var departments = await _unitOfWork.DepartmentRepository.GetIQueryable().Where(D => !D.IsDeleted && (string.IsNullOrEmpty(search) || (D.Name.ToLower().Contains(search.ToLower())))).Select(de => new DepartmentDto
            {
                Id = de.Id,
                Code = de.Code,
                Name = de.Name,
                CreationDate = de.CreationDate,
            }).AsNoTracking().ToListAsync();
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

        public async Task<DepartmentDetailsDto?> GetDepartmentsByIdAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id);
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

        public async Task<int> CreateDepartmentAsync(CreatedDepartmentDto departmentDto)
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
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<int> UpdateDepartmentAsync(UpdatedDepartmentDto departmentDto)
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
            return await _unitOfWork.CompleteAsync();
        }
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department = await departmentRepo.GetAsync(id);
            if (department is null)
                return false;
            departmentRepo.Delete(department);
            return await _unitOfWork.CompleteAsync() > 0;
        }

       
    }
}
