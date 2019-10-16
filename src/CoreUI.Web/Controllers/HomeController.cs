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
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CoreUI.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        const string SessionEmail = "_Email";
        const string SessionName = "_Name";
        const string SessionEmployeeId = "_Id";
        const string SessionAcessLevel = "_IdAccessLevel";
        const string SessionInvalid = "false";
        const string SessionExpired = "false";
        const string SessionTotalBells = "false";

        public HomeController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, IConfiguration config)
        {
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
            _config = config;
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

        public IActionResult AlertsBell()
        {
            
            return View(_hourService);
        }

        public IActionResult ChangePassword()
        {
            GetSessions();
            return View();
        }

        public IActionResult UpdatePassword(string email,string password)
        {
            GetSessions();

            if (ViewBag.Email == null)
            {
                return ExpiredSession();
            }

            string queryString = "update dev_jump.Employee set password = '" + password + "', change_password = 0 where Email = '" + email + "'";
            ExecuteQuery(queryString);
            return RedirectToAction("Index", "Hours");
        }

        public void ExecuteQuery(string query)
        {
            string conn = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");
            MySqlConnection connection = new MySqlConnection(conn);
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidLogin([Bind("Email", "Password")] Employee employee)
        {

            if (_context.Employee.Count(emp => emp.Email == employee.Email && emp.Password == employee.Password && emp.Active == 1) == 0 && !ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home", "true");
            }

            string queryString = "SELECT * FROM dev_jump.Employee where email = '" + employee.Email + "' and active = 1";
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = command;

            DataTable dt = new DataTable();

            da.Fill(dt);
            object id = dt;

            int idEmployee = (int)dt.Rows[0]["id"];
            string NameEmployee = dt.Rows[0]["Name"].ToString();
            int accessLEvel = (int)dt.Rows[0]["Access_LevelId"];

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
                HttpContext.Session.SetInt32(SessionAcessLevel, accessLEvel);
            }

            if (_context.Employee.Count(emp => emp.Email == employee.Email && emp.Password == employee.Password && emp.Active == 1 && emp.Change_Password == 1) == 1)
            {
                ViewData["Email"] = employee.Email;
                return RedirectToAction(nameof(ChangePassword));
            }

            return RedirectToAction("Index", "Hours");

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logoff()
        {
            HttpContext.Session.Remove(SessionEmail);
            HttpContext.Session.Remove(SessionEmployeeId);
            HttpContext.Session.Remove(SessionName);
            HttpContext.Session.Remove(SessionAcessLevel);
            HttpContext.Session.Remove(SessionTotalBells);
            HttpContext.Session.Remove(SessionEmployeeId);
            return RedirectToAction("Index", "Home");

        }

        public void GetSessions()
        {

            ViewBag.Email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.Id = HttpContext.Session.GetInt32(SessionEmployeeId);
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.AcessLevel = HttpContext.Session.GetInt32(SessionAcessLevel);
            ViewBag.TotalMessagesBells = HttpContext.Session.GetInt32(SessionTotalBells);

        }

        public IActionResult ExpiredSession()
        {
            HttpContext.Session.SetString(SessionExpired, "true");
            return RedirectToAction("Index", "Home", "Index");
        }

    }
}
