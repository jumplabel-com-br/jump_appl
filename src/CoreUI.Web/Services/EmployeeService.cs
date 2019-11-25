using CoreUI.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> FindAllAsync()
        {
            return await _context.Employee
                .Where(x => x.Active == 1)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Employee>> FindEmployeesAsync()
        {
            return await _context.Employee
                //.Where(x => x.Active == 1)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Employee>> FindEmployeeAsync(int? id)
        {
            return await _context.Employee
                .Where(x => x.Id == id && x.Active == 1)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
