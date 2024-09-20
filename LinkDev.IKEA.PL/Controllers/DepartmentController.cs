using LinkDev.IKEA.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    public class DepartmentController(IDepartmentServices departmentServices) : Controller
    {
        private readonly IDepartmentServices _departmentServices = departmentServices;

        public IActionResult Index()
        {
            var departments = _departmentServices.GetAllDepartments();
            return View(departments);
        }
    }
}
