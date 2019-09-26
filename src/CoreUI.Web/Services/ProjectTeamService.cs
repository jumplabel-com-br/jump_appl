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

        public async Task<List<Project_team>> FindAllAsync()
        {

            var result = from obj in _context.Project_team select obj;
            /*
            return await result
                .Include(x => x.Employee)
                .Include(x => x.Project)
                .ToListAsync();
                */
             return await _context.Project_team.OrderBy(x => x.Id).ToListAsync();

        }
    }
}
