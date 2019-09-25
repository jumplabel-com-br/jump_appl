using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreUI.Web.Models;

namespace CoreUI.Web.Data
{
    public class SeedingService
    {
        private ApplicationDbContext _context;

        public SeedingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            
            if (_context.Employee.Any() || _context.Hour.Any() || _context.Project.Any())
            {
                return; //DB has been seeded
            }

            _context.SaveChanges();
        }
    }
}
