using AutoMapper;
using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.DAL.Entities.Departments;
using LinkDev.IKEA.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LinkDev.IKEA.PL.Controllers
{
    public class DepartmentController
        (IDepartmentServices _departmentServices,
        IMapper _mapper,
        ILogger<DepartmentController> _logger,
        IWebHostEnvironment _webHostEnvironment) : Controller
    {
        //#region Services
        //private readonly IDepartmentServices _departmentServices = departmentServices;
        //private readonly ILogger<DepartmentController> _logger = logger;
        //private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        //#endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string search)
        {
            #region ViewData vs ViewBag
            //// view's Dictionary : Pass data from Controller [Action] to view ( from view -> [partial view, Layout]

            ///// 1. ViewData:  is a dictionary type property (introduced in .net framework 3.5)
            ///// => helps us to transfer data from controller[Action] to view

            //ViewData["Message"] = "Hello from View Data";
            //ViewData["obj"] = "Hello from View Data";
            ///// 2. ViewBag:  is a Dynamic type property (introduced in .net framework 4 based on Dynamic Feature)
            ///// => helps us to transfer data from controller[Action] to view

            //ViewBag.Message = "Hello from View Bag";
            //ViewBag.obj02 = "Hello from View Data";
            //ViewBag.obj02 = new { Id = 1, Name = "Mohamed" }; 
            #endregion

            var departments = await _departmentServices.GetDepartmentsAsync(search);
            return View(departments);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var department = await _departmentServices.GetDepartmentsByIdAsync(id.Value);

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
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {

            if (!ModelState.IsValid)
                return View(departmentVM);

            var message = String.Empty;

            try
            {

                var department = _mapper.Map<CreatedDepartmentDto>(departmentVM);

                /// var department = (new CreatedDepartmentDto
                /// {
                ///     Code = departmentVM.Code,
                ///     Name = departmentVM.Name,
                ///     CreationDate = departmentVM.CreationDate,
                ///     Description = departmentVM.Description,
                /// });
                
                var created = await _departmentServices.CreateDepartmentAsync(department) > 0;
                ///  TempData:  is a dictionary type property (introduced in .net framework 3.5)
                ///  => used to transfer data between 2 Consuctive Requests

                if (!created)
                {
                    message = "Department is not Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentVM);
                }
            }
            catch (Exception ex)
            {

                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);

                // 2.Set Message
                message = _webHostEnvironment.IsDevelopment() ? ex.Message : "an Error has been occured during creating the department :(";

                return RedirectToAction(nameof(Index));
            }
           return Redirect(nameof(Index));
        }
        #endregion


        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var department = await _departmentServices.GetDepartmentsByIdAsync(id.Value);

            if (department is null)
                return NotFound();

            var departmentVM = _mapper.Map<DepartmentViewModel>(department);
            return View(departmentVM);

            ///return View(new DepartmentViewModel
            ///{
            ///    Code = department.Code,
            ///    Name = department.Name,
            ///    Description = department.Description,
            ///    CreationDate = department.CreationDate,
            ///});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentViewModel departmentVM)
        {
            if(id is null)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(departmentVM);

            var Message = string.Empty;
            try
            {
                var departmentDto = _mapper.Map<UpdatedDepartmentDto>(departmentVM);

                /// var departmentDto = new UpdatedDepartmentDto
                /// {
                ///     Id = id.Value,
                ///     Code = departmentVM.Code,
                ///     Name = departmentVM.Name,
                ///     Description = departmentVM.Description,
                ///     CreationDate = departmentVM.CreationDate,
                /// };

                var result = await _departmentServices.UpdateDepartmentAsync(departmentDto) > 0;

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
        public async Task<IActionResult> Delete([FromRoute] int? id)
        {
            if (id == null)
                return BadRequest();
            var department = await _departmentServices.GetDepartmentsByIdAsync(id.Value);

            if (department == null)
                return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentServices.GetDepartmentsByIdAsync(id);
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

                var IsDeleted = await _departmentServices.DeleteDepartmentAsync(id);

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
