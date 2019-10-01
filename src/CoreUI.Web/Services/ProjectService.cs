using CoreUI.Web.Models;
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
            return await obj.OrderBy(x => x.Project_Name).ToListAsync();
            //return await _context.Project
            //  .OrderBy(x => x.Project_Name).ToListAsync();
        }

        public async Task<List<Project>> FindPerEmployeeAsync() 
        {
            var obj = from project in _context.Project
                      join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                      where DateTime.Now.Date <= projectTeam.End_Date

                      select project;

            return await obj
                .OrderBy(x => x.Project_Name).ToListAsync();
        }

    }
}
