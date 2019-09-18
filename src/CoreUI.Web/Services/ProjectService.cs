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
            return await _context.Project.OrderBy(x => x.Project_Name).ToListAsync();
        }

    }
}
