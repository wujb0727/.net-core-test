using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelloWorld.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace HelloWorld.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HelloWorldDBContext _context;

        public HomeController(HelloWorldDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            //return Content("hello world;这条消息来自使用了 Action Result 的 Home 控制器!");

            //var employee = new Employee { ID = 1, Name = "阿黄" };
            ////return new ObjectResult(employee);
            //ViewData["emp"] = employee.Name;
            //ViewBag.emp = employee.Name;
            //return View(employee);

            var model = new HomePageViewModel();

            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            model.Employees = sqlData.GetEmployees();

            return View(model);

        }

        public ViewResult Detail(int id)
        {
            //var model = new HomePageViewModel();

            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            Employee employee = sqlData.GetEmployee(id);

            return View(employee);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeEditViewModel input)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Name = input.Name
                };

                SQLEmployeeData sqlData = new SQLEmployeeData(_context);
                sqlData.Add(employee);

                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            //var model = new HomePageViewModel();

            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            Employee employee = sqlData.GetEmployee(id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(int id, EmployeeEditViewModel input)
        {
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            var employee = sqlData.GetEmployee(id);

            if (employee != null && ModelState.IsValid)
            {
                employee.Name = input.Name;
                _context.SaveChanges();
                return RedirectToAction("Detail", new { id = employee.ID });
            }
            return View(employee);
        }

    }

    public class SQLEmployeeData
    {
        private HelloWorldDBContext _context { get; set; }

        public SQLEmployeeData(HelloWorldDBContext context)
        {
            _context = context;
        }

        public void Add(Employee employee)
        {
            _context.Add(employee);
            _context.SaveChanges();
        }

        public Employee GetEmployee(int ID)
        {
            return _context.Employees.FirstOrDefault(e => e.ID == ID);
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees.ToList<Employee>();
        }

    }

    public class HomePageViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
    }

    public class EmployeeEditViewModel
    {
        [Required, MaxLength(80)]
        public string Name { get; set; }
    }

}
