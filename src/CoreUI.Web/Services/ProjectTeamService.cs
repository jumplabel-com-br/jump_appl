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

        public async Task<List<ListProjectTeam>> FindAllAsync()
        {

            //var result = from obj in _context.Project_team select obj;

            var result = from projectTeam in _context.Project_team
                         join project in _context.Project on projectTeam.Project_Id equals project.Id
                         join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id

                         select
                         new ListProjectTeam
                         {
                             Id = projectTeam.Id,
                             Employee = employee.Name,
                             Project = project.Project_Name,
                             Start = projectTeam.Start_Date,
                             End = projectTeam.End_Date
                         };

            return await result
                .OrderBy(x => x.Project)
                .ToListAsync();

             //return await _context.Project_team.OrderBy(x => x.Id).ToListAsync();

        }
    }
}
