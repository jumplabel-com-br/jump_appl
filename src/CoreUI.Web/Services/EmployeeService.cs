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
            var result = from employees in _context.Employee
                         select new Employee()
                         {
                             Id = employees.Id == null ? 0 : employees.Id,
                             Email = employees.Email == null ? "" : employees.Email,
                             Name = employees.Name == null ? "" : employees.Name,
                             Document = employees.Document == null ? "" : employees.Document,
                             Contract_Mode = employees.Contract_Mode == null ? "" : employees.Contract_Mode,
                             Active = employees.Active == null ? 0 : employees.Active,
                             Appointment = employees.Appointment == null ? 0 : employees.Appointment,
                             Password = employees.Password == null ? "" : employees.Password,
                             Change_Password = employees.Change_Password == null ? 0 : employees.Change_Password,
                             Access_LevelId = employees.Access_LevelId == null ? 0 : employees.Access_LevelId
                         };

            return await result
                //.Where(x => x.Active == 1)
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

        public async Task<List<Employee>> FindEmployeesActivesAsync()
        {
            return await _context.Employee
                .Where(x => x.Active == 1)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Employee>> FindAllManagersAsync()
        {
            return await _context.Employee
                .Where(x => x.Active == 1 && (x.Access_LevelId == 1 || x.Access_LevelId == 2))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Employee>> FindDifferenceEmployeeAsync()
        {
            return await _context.Employee
                .Where(x => x.Active == 1 && (x.Access_LevelId != 3))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
