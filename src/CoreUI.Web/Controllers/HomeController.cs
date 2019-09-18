using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreUI.Web.Models;
using Microsoft.AspNetCore.Http;
using CoreUI.Web.Services;
using MySql.Data.MySqlClient;
using System.Data;

namespace CoreUI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";

        public HomeController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour)
        {
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
        }




        public IActionResult Index()
        {
            ViewBag.Invalid = HttpContext.Session.GetString(SessionInvalid);
            ViewBag.Experid = HttpContext.Session.GetString(SessionExpired);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidLogin([Bind("Email", "Password")] Employee employee)
        {

            if (_context.Employee.Count(emp => emp.Email == employee.Email && emp.Password == employee.Password) == 0 && !ModelState.IsValid)
            {

                return RedirectToAction("Index", "Home", "true");
            }

            string queryString = "SELECT * FROM dev_jump.Employee where email = '" + employee.Email + "'";
            string connString = "Server = datajump.ci9niqaqsefm.us-east-1.rds.amazonaws.com; Database = dev_jump; Uid = dev_jump; Pwd = dev_jump;";

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = command;

            DataTable dt = new DataTable();

            da.Fill(dt);
            object id = dt;

            int idEmployee = (int)dt.Rows[0]["id"];
            string NameEmployee = dt.Rows[0]["Name"].ToString();

            /*
            if (!string.IsNullOrEmpty(idEmployee))
            {
                ViewBag.idEmployee = idEmployee;
                ViewBag.Name = NameEmployee;
            }
            */

            //hour.Employee.Where(emp => emp.Email == employee.Email);


            if (ViewBag.Email == null)
            {

                HttpContext.Session.SetString(SessionEmail, employee.Email);
                HttpContext.Session.SetInt32(SessionEmployeeId, idEmployee);
                HttpContext.Session.SetString(SessionName, NameEmployee);
            }

            return RedirectToAction("Index", "Hours");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logoff()
        {
            HttpContext.Session.Remove(SessionEmployeeId);
            return RedirectToAction("Index", "Home");

        }

    }
}
