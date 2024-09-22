using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.DAL.Entities.Departments;
using LinkDev.IKEA.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    public class DepartmentController
        (IDepartmentServices departmentServices,
        ILogger<DepartmentController> logger,
        IWebHostEnvironment webHostEnvironment) : Controller
    {
        #region Services
        private readonly IDepartmentServices _departmentServices = departmentServices;
        private readonly ILogger<DepartmentController> _logger = logger;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentServices.GetAllDepartments();
            return View(departments);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var department = _departmentServices.GetDepartmentsById(id.Value);

            if (department is { })
                return View(department);

            return NotFound();
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentViewModel departmentVM)
        {

            if (!ModelState.IsValid)
                return View(departmentVM);

            var Message = String.Empty;


            try
            {


                var Result = _departmentServices.CreateDepartment(new CreatedDepartmentDto
                {
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    CreationDate = departmentVM.CreationDate,
                    Description = departmentVM.Description,
                });

                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {

                    Message = "Department is not Created";

                    ModelState.AddModelError(string.Empty, Message);

                    return View(departmentVM);

                }


            }
            catch (Exception ex)
            {

                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);

                // 2.Set Message
                Message = _webHostEnvironment.IsDevelopment() ? Message = ex.Message : "an Error has been occured during creating the department :(";

            }
            ModelState.AddModelError(String.Empty, Message);
            return View(departmentVM);
        }
        #endregion


        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var department = _departmentServices.GetDepartmentsById(id.Value);

            if (department is null)
                return NotFound();

            return View(new DepartmentViewModel
            {
                Description = department.Description,
                CreationDate = department.CreationDate,
                Code = department.Code,
                Name = department.Name,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);

            var Message = string.Empty;
            try
            {
                var departmentDto = new UpdatedDepartmentDto
                {
                    Id = id,
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    CreationDate = departmentVM.CreationDate,
                    Description = departmentVM.Description,
                };

                var result = _departmentServices.UpdateDepartment(departmentDto) > 0;

                if (result)
                    return RedirectToAction("Index");

                Message = "an Error has been occured during updating the department :(";
            }
            catch (Exception ex)
            {

                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);

                // 2.Set Message
                Message = _webHostEnvironment.IsDevelopment() ? Message = ex.Message : "an Error has been occured during updating the department :(";

            }

            ModelState.AddModelError(String.Empty, Message);
            return View(departmentVM);

        }

        #endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete([FromRoute] int? id)
        {
            if (id == null)
                return BadRequest();
            var department = _departmentServices.GetDepartmentsById(id.Value);

            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var department = _departmentServices.GetDepartmentsById(id);
            var Message = string.Empty;
            if (department == null)
                return NotFound();
            try
            {
                var departmentDto = new UpdatedDepartmentDto
                {
                    Id = id,
                    Code = department.Code,
                    Name = department.Name,
                    CreationDate = department.CreationDate,
                    Description = department.Description,
                };

                var IsDeleted = _departmentServices.DeleteDepartment(id);

                if (IsDeleted)
                    return RedirectToAction("Index");

                Message = "an Error has been occured during Deleting the department :(";
            }
            catch (Exception ex)
            {

                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);

                // 2.Set Message
                Message = _webHostEnvironment.IsDevelopment() ? Message = ex.Message : "an Error has been occured during Deleting the department :(";

            }

            ModelState.AddModelError(String.Empty, Message);
            return View(department);
        } 
        #endregion
    }
}
