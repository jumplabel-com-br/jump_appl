using CoreUI.Web.Models;
using CoreUI.Web.Models.List;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;


        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> FindAllAsync()
        {

            var obj = from project in _context.Project select project;
            return await obj
                .Where(x => x.Project_Name != "" && x.Project_Name != null && x.Active == 1)
                .OrderBy(x => x.Project_Name)
                .Distinct()
                .ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

        public async Task<List<Project>> FindProjectAsync(int? id)
        {

            var obj = from project in _context.Project select project;
            return await obj
                .Where(x => x.Id == id && x.Active == 1)
                .ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

        public async Task<List<Project>> FindPerEmployeeAsync(int employeeId, int accessLevel)
        {
            
                var result = from project in _context.Project
                          join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                          join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id
                          where employee.Id == employeeId

                          select project;


                return await result
                    .OrderBy(x => x.Project_Name)
                    .Distinct()
                    .ToListAsync();
            /*
            var result = from project in _context.Project
                         join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                         join employee in _context.Employee on projectTeam.Employee_Id equals employee.Id
                         where DateTime.Now.Date >= projectTeam.Start_Date && DateTime.Now.Date <= projectTeam.End_Date

                         select project;

            return await result
                .OrderBy(x => x.Project_Name)
                .Distinct()
                .ToListAsync();*/

        }

        public async Task<List<ListProject>> FindAllToListAsync()
        {

            var obj = from project in _context.Project
                      join client in _context.Client on project.Client_Id equals client.Id

                      select new ListProject
                      {
                          Id = project.Id,
                          Project = project.Project_Name,
                          Client = client.Name,
                          Status = project.Active.ToString(),
                          CostCenter = project.Cost_Center_Id

                      };

            return await obj.OrderBy(x => x.Project).ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

    }
}
