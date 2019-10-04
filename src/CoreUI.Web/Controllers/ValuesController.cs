using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CoreUI.Web.Models;
using CoreUI.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace JsonAlertsBellPerEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectService _projectService;
        private readonly EmployeeService _employeeService;
        private readonly HourService _hourService;
        private readonly IConfiguration _config;

        public ValuesController(ApplicationDbContext context, ProjectService project, EmployeeService employee, HourService hour, IConfiguration config)
        {
            _context = context;
            _projectService = project;
            _employeeService = employee;
            _hourService = hour;
            _config = config;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
