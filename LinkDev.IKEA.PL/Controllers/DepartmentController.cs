using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    public class DepartmentController
        (IDepartmentServices departmentServices,
        ILogger<DepartmentController> logger,
        IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IDepartmentServices _departmentServices = departmentServices;
        private readonly ILogger<DepartmentController> _logger = logger;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        public IActionResult Index()
        {
            var departments = _departmentServices.GetAllDepartments();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(CreatedDepartmentDto department)
        {

            if (!ModelState.IsValid)
                return View(department);

            var Message = String.Empty;


            try
            {


                var Result = _departmentServices.CreateDepartment(department);

                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {

                    Message = "Department is not Created";

                    ModelState.AddModelError(string.Empty, Message);

                    return View(department);

                }


            }
            catch (Exception ex)
            {


                // 1. Log Exceptions
                _logger.LogError(ex, ex.Message);


                // 2.Set Message
                if (_webHostEnvironment.IsDevelopment())
                {
                    Message = ex.Message;
                    return View(department);
                }
                else
                {
                    Message = "Department is not Created";
                    return View("Error", Message);
                }


            }
        }

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
        
    }
}
