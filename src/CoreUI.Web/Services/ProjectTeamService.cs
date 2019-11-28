using CoreUI.Web.Models;
using CoreUI.Web.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class ProjectTeamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;


        public ProjectTeamService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<List<ListProjectTeam>> FindAllAsync(int accessLevel, int employeeId)
        {

            //var result = from obj in _context.Project_team select obj;

            var result = from projectTeam in _context.Project_team
                         join project in _context.Project on projectTeam.Project_Id equals project.Id
                         join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id
                         join client in _context.Client on project.Client_Id equals client.Id

                         select
                         new ListProjectTeam
                         {
                             Id = projectTeam.Id == null ? 0 : projectTeam.Id,
                             Client = client.Name == null ? "" : client.Name,
                             ClientId = client.Id == null ? 0 : client.Id,
                             Employee = employee.Name == null ? "" : employee.Name,
                             EmployeeId = employee.Id == null ? 0 : employee.Id,
                             Project = project.Project_Name == null ? "" : project.Project_Name,
                             ProjectId = project.Id == null ? 0 : project.Id,
                             Start = projectTeam.Start_Date == null ? DateTime.Now : projectTeam.Start_Date,
                             End = projectTeam.End_Date == null ? DateTime.Now : projectTeam.End_Date,
                             AccessLevel = employee.Access_LevelId == null ? 0 : employee.Access_LevelId,
                             Project_Manager = project.Project_Manager_Id == null ? 0 : project.Project_Manager_Id
                         };

            if (accessLevel == 2)
            {
                result = result.Where(x => x.AccessLevel == accessLevel && x.Project_Manager == employeeId);
            }

            return await result
                .OrderBy(x => x.Client)
                .OrderBy(x => x.Project)
                .ToListAsync();

            //return await _context.Project_team.OrderBy(x => x.Id).ToListAsync();

        }

        public bool FindEndProject(DateTime endProjet)
        {

            if (_context.Project_team.Count(x => x.End_Date == endProjet) == 1)
            {
                return false;
            }

            return true;
        }
    }
}
