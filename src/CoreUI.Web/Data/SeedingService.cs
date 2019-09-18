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

            Employee ts1 = new Employee(1, "teste", "teste1", "teste2", 1, 200.0, 0, "xyz123", 0);
            Project p = new Project(1, "Teste", 1, 1, 0);

            _context.Employee.AddRange(ts1);
            _context.Project.AddRange(p);

            _context.SaveChanges();
        }
    }
}
