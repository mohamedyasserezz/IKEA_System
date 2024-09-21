﻿using LinkDev.IKEA.BLL.Models.Employees;
using LinkDev.IKEA.BLL.Services.Employees;
using LinkDev.IKEA.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    public class EmployeeController
        (IEmployeeServices employeeServices,
        ILogger<EmployeeController> logger,
        IWebHostEnvironment webHostEnvironment) : Controller
    {
        #region Services
        private readonly IEmployeeServices _employeeServices = employeeServices;
        private readonly ILogger<EmployeeController> _logger = logger;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeServices.GetAllEmployees();
            return View(employees);
        }
        #endregion

        #region Details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var Employee = _employeeServices.GetEmployeesById(id.Value);

            if (Employee is { })
                return View(Employee);

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
        public IActionResult Create(CreatedEmployeeDto Employee)
        {

            if (!ModelState.IsValid)
                return View(Employee);

            var Message = String.Empty;


            try
            {

                var Result = _employeeServices.CreateEmployee(Employee);

                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {

                    Message = "Employee is not Created";

                    ModelState.AddModelError(string.Empty, Message);

                    return View(Employee);

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
            return View(Employee);
        }
        #endregion


        //#region Edit
        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //        return BadRequest();

        //    var Employee = _employeeServices.GetEmployeesById(id.Value);

        //    if (Employee is null)
        //        return NotFound();

        //    return View(new EmployeeEditViewModel
        //    {
        //        Description = Employee.,
        //        CreationDate = Employee.CreationDate,
        //        Code = Employee.Code,
        //        Name = Employee.Name,
        //    });
        //}

        //[HttpPost]
        //public IActionResult Edit([FromRoute] int id, EmployeeEditViewModel employeeVM)
        //{
        //    if (!ModelState.IsValid)
        //        return View(employeeVM);

        //    var Message = string.Empty;
        //    try
        //    {
        //        var EmployeeDto = new UpdatedEmployeeDto
        //        {
        //            Id = id,
        //            Code = employeeVM.Code,
        //            Name = employeeVM.Name,
        //            CreationDate = employeeVM.CreationDate,
        //            Description = employeeVM.Description,
        //        };

        //        var result = _dmployeeServices.UpdateEmployee(EmployeeDto) > 0;

        //        if (result)
        //            return RedirectToAction("Index");

        //        Message = "an Error has been occured during updating the Employee :(";
        //    }
        //    catch (Exception ex)
        //    {

        //        // 1. Log Exceptions
        //        _logger.LogError(ex, ex.Message);

        //        // 2.Set Message
        //        Message = _webHostEnvironment.IsDevelopment() ? Message = ex.Message : "an Error has been occured during updating the Employee :(";

        //    }

        //    ModelState.AddModelError(String.Empty, Message);
        //    return View(EmployeeVM);

        //}

        //#endregion

        #region Delete
        [HttpGet]
        public IActionResult Delete([FromRoute] int? id)
        {
            if (id == null)
                return BadRequest();
            var Employee = _employeeServices.GetEmployeesById(id.Value);

            if (Employee == null)
                return NotFound();

            return View(Employee);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
          
            var Message = string.Empty;
            
            try
            {
                var result = _employeeServices.DeleteEmployee(id);
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