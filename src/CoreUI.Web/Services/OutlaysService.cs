using CoreUI.Web.Models;
using CoreUI.Web.Models.List;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class OutlaysService
    {
        private readonly ApplicationDbContext _context;

        public OutlaysService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListOutlays>> FindAllAsync(int? status, int? clients, int? projects, int? employees, int? month, int? year)
        {
            var result = from outlays in _context.Outlays
                         join clientes in _context.Client
                            on outlays.Client_Id equals clientes.Id
                         join projetos in _context.Project
                            on outlays.Project_Id equals projetos.Id
                         join funcionarios in _context.Employee
                            on outlays.Employee_Id equals funcionarios.Id
                         join projectTeam in _context.Project_team
                            on projetos.Id equals projectTeam.Project_Id

                         select new ListOutlays
                         {
                             Id = outlays.Id,
                             Id_Employee = funcionarios.Id,
                             Employee = funcionarios.Name,
                             Id_Project = projetos.Id,
                             Project = projetos.Project_Name,
                             Id_Client = clientes.Id,
                             Client = clientes.Name,
                             Date = outlays.Date,
                             NoteNumber = outlays.NoteNumber,
                             NoteValue = outlays.NoteValue,
                             Description = outlays.Description,
                             File = outlays.File,
                             Status = outlays.Status
                         };

            if (status.HasValue)
            {
                result = result.Where(x => x.Status == status).Distinct();
            }

            if (clients.HasValue)
            {
                result = result.Where(x => x.Id_Client == clients).Distinct();
            }

            if (projects.HasValue)
            {
                result = result.Where(x => x.Id_Project == projects).Distinct();
            }

            if (employees.HasValue)
            {
                result = result.Where(x => x.Id_Employee == employees).Distinct();
            }

            if (month.HasValue)
            {
                result = result.Where(x => x.Date.Month == month).Distinct();
            }

            if (year.HasValue)
            {
                result = result.Where(x => x.Date.Year == year).Distinct();
            }

            return await result
                .OrderBy(x => x.Date)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<ListOutlays>> ReportsFindAllAsync()
        {
            var result = from outlays in _context.Outlays
                         join clients in _context.Client
                            on outlays.Client_Id equals clients.Id
                         join projects in _context.Project
                            on outlays.Project_Id equals projects.Id
                         join employees in _context.Employee
                            on outlays.Employee_Id equals employees.Id
                         join projectTeam in _context.Project_team
                            on projects.Id equals projectTeam.Project_Id
                         where outlays.Status == 2

                         select new ListOutlays
                         {
                             Id = outlays.Id,
                             Employee = employees.Name,
                             Project = projects.Project_Name,
                             Client = clients.Name,
                             Date = outlays.Date,
                             NoteNumber = outlays.NoteNumber,
                             NoteValue = outlays.NoteValue,
                             Description = outlays.Description,
                             File = outlays.File,
                             Status = outlays.Status
                         };

            return await result
                .OrderBy(x => x.Date)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<ListOutlays>> FindPerEmployee(int employeeId)
        {
            var result = from outlays in _context.Outlays
                         join clients in _context.Client
                            on outlays.Client_Id equals clients.Id
                         join projects in _context.Project
                            on outlays.Project_Id equals projects.Id
                         join employees in _context.Employee
                            on outlays.Employee_Id equals employees.Id
                         join projectTeam in _context.Project_team
                            on projects.Id equals projectTeam.Project_Id
                         where outlays.Employee_Id == employeeId

                         select new ListOutlays
                         {
                             Id = outlays.Id,
                             Employee = employees.Name,
                             Project = projects.Project_Name,
                             Client = clients.Name,
                             Date = outlays.Date,
                             NoteNumber = outlays.NoteNumber,
                             NoteValue = outlays.NoteValue,
                             Description = outlays.Description,
                             File = outlays.File,
                             Status = outlays.Status
                         };

            return await result
                .OrderBy(x => x.Date)
                .Distinct()
                .ToListAsync();
        }
    }
}
