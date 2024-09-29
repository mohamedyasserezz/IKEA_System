using LinkDev.IKEA.BLL.Common.Services.Attachments;
using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
namespace LinkDev.IKEA.BLL.Services.Employees
{
    public class EmployeeServices(IUnitOfWork _unitOfWork,
        IAttachmentService _attachmentService) : IEmployeeServices
    {

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(string search)
        {


            var employees = await _unitOfWork.EmployeeRepository.GetIQueryable()
                .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(search) || E.Name.ToLower().Contains(search.ToLower()))).Include(E => E.Department)
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
                    Image = EmployeeDto.Image,

                }).ToListAsync();

            return employees;
        }

        public async Task<EmployeeDetailsDto?> GetEmployeesByIdAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);
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
                    Image = employee.Image,

                };
            return null;

        }

        public async Task<int> CreateEmployeeAsync(EmployeeViewModel EmployeeDto)
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

            if(EmployeeDto.Image is not null) 
                employee.Image = _attachmentService.Upload(EmployeeDto.Image, "Images");
             _unitOfWork.EmployeeRepository.Add(employee);
            return await _unitOfWork.CompleteAsync();
        }
        public async Task<int> UpdateEmployeeAsync(UpdatedEmployeeDto EmployeeDto)
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
                Image = EmployeeDto.Image,
            };

            _unitOfWork.EmployeeRepository.Update(employee);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employeeRepo = _unitOfWork.EmployeeRepository;
            var employee = await employeeRepo.GetAsync(id);
            if (employee == null)
                return false;
            employeeRepo.Delete(employee);
            return await _unitOfWork.CompleteAsync() > 0;

        }



    }
}
