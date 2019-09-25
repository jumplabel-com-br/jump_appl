using CoreUI.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class AccessLevelService
    {
        private readonly ApplicationDbContext _context;


        public AccessLevelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Access_Level>> FindAllAsync()
        {
            return await _context.Access_Level.OrderBy(x => x.Access_level).ToListAsync();
        }
    }
}
