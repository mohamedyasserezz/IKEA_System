using AutoMapper;
using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.BLL.Services.Employees;
using LinkDev.IKEA.DAL.Entities.Employees;
using LinkDev.IKEA.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    [Authorize]
    public class EmployeeController
        (IEmployeeServices _employeeServices,
        IDepartmentServices _departmentServices,
        IMapper _mapper,
        ILogger<EmployeeController> _logger,
        IWebHostEnvironment _webHostEnvironment
        ) : Controller
    {
        //#region Services
        //private readonly IEmployeeServices _employeeServices = employeeServices;
        //private readonly ILogger<EmployeeController> _logger = logger;
        //private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        //#endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string search)
        {
            var employees = await _employeeServices.GetEmployeesAsync(search);
            return View(employees);
        }
        #endregion

        //#region Search
        //[HttpGet]
        //public IActionResult Search([FromQuery] string search)
        //{
        //    var employees = _employeeServices.GetEmployees(search);
        //    return PartialView("Partials/EmployeeTablePartial", employees);
        //}
        //#endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var Employee = await _employeeServices.GetEmployeesByIdAsync(id.Value);

            if (Employee is { })
                return View(Employee);

            return NotFound();
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create([FromServices] IDepartmentServices _departmentServices)
        {
            ViewData["Departments"] = await _departmentServices.GetDepartmentsAsync(null!);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel EmployeeVM)
        {

            if (!ModelState.IsValid)
                return View(EmployeeVM);

            var Message = String.Empty;


            try
            {
                var Result = await _employeeServices.CreateEmployeeAsync(EmployeeVM);

                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {

                    Message = "Employee is not Created";

                    ModelState.AddModelError(string.Empty, Message);

                    return View(EmployeeVM);

                }


            }
            catch (Exception ex)
            {

                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);

                // 2.Set Message
                Message = _webHostEnvironment.IsDevelopment() ? Message = ex.Message : "an Error has been occured during creating the Employee :(";

            }
            ModelState.AddModelError(String.Empty, Message);
            return View(EmployeeVM);
        }
        #endregion


        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var Employee = await _employeeServices.GetEmployeesByIdAsync(id.Value);
            ViewData["Departments"] = await _departmentServices.GetDepartmentsAsync(null!);
            if (Employee is null)
                return NotFound();
            var EmployeeVM = _mapper.Map<EmployeeViewModel>(Employee);
            return View(EmployeeVM);

            ///return View(new UpdatedEmployeeDto
            ///{
            ///    EmployeeType = Employee.EmployeeType,
            ///    Gender = Employee.Gender,
            ///    Address = Employee.Address,
            ///    Age = Employee.Age,
            ///    Email = Employee.Email,
            ///    HiringDate = Employee.HiringDate,
            ///    IsActive = Employee.IsActive,
            ///    Name = Employee.Name,
            ///    PhoneNumber = Employee.PhoneNumber,
            ///    Salary = Employee.Salary,
            ///});
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (!ModelState.IsValid)
                return View(employeeVM);

            var Message = string.Empty;
            try
            {
                var employeeDto = _mapper.Map<UpdatedEmployeeDto>(employeeVM);

                var result = await _employeeServices.UpdateEmployeeAsync(employeeDto) > 0;

                if (result)
                    return RedirectToAction("Index");

                Message = "an Error has been occured during updating the Employee :(";
            }
            catch (Exception ex)
            {

                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);

                // 2.Set Message
                Message = _webHostEnvironment.IsDevelopment() ? Message = ex.Message : "an Error has been occured during updating the Employee :(";

            }

            ModelState.AddModelError(String.Empty, Message);
            return View(employeeVM);

        }

        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] int? id)
        {
            if (id == null)
                return BadRequest();
            var Employee = await _employeeServices.GetEmployeesByIdAsync(id.Value);

            if (Employee == null)
                return NotFound();

            return View(Employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            var Message = string.Empty;

            try
            {
                var result = await _employeeServices.DeleteEmployeeAsync(id);
                if (result)
                    return RedirectToAction("Index");



                Message = "an Error has been occured during Deleting the Employee :(";
            }
            catch (Exception ex)
            {

                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);

                // 2.Set Message
                Message = _webHostEnvironment.IsDevelopment() ? Message = ex.Message : "an Error has been occured during Deleting the Employee :(";

            }

            ModelState.AddModelError(String.Empty, Message);
            return View();
        }
        #endregion
    }
}
