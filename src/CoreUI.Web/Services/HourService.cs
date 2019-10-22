using CoreUI.Web.Models;
using CoreUI.Web.Models.List;
using CoreUI.Web.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class HourService
    {
        private readonly ApplicationDbContext _context;

        public HourService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListHour>> FindAllAsync()
        {
            var result = from hour in _context.Hour
                         join projects in _context.Project on hour.Id_Project equals projects.Id
                         join clients in _context.Client on projects.Client_Id equals clients.Id
                         join employees in _context.Employee on hour.Employee_Id equals employees.Id
                         orderby hour.Date ascending
                         orderby clients.Name ascending

                         select new ListHour
                         {
                             Id = hour.Id,
                             Project = hour.Project,
                             Date = hour.Date,
                             Start_Time = hour.Start_Time,
                             Stop_Time = hour.Stop_Time,
                             Start_Time_2 = hour.Start_Time_2,
                             Stop_Time_2 = hour.Stop_Time_2,
                             Activies = hour.Activies,
                             Total_Activies_Hours = hour.Total_Activies_Hours,
                             Consultant = employees.Email.Replace("@jumplabel.com.br", ""),
                             Creation_Date = hour.Creation_Date,
                             Id_Project = hour.Id_Project,
                             Employee_Id = hour.Employee_Id,
                             Arrival_Time = hour.Arrival_Time,
                             Beginning_Of_The_Break = hour.Beginning_Of_The_Break,
                             End_Of_The_Break = hour.End_Of_The_Break,
                             Exit_Time = hour.Exit_Time,
                             Total_Hours_In_Activity = hour.Total_Hours_In_Activity,
                             Approval = hour.Approval,
                             Approver = hour.Approver,
                             Client = clients.Name
                         };

            return await result
                //.OrderBy(x => x.Client)
                //.OrderBy(x => x.Project)
                .OrderBy(x => x.Consultant)
                .OrderBy(x => x.Date)
                .ToListAsync();

        }

        public async Task<List<ListHour>> FindAllPerEmployeeAsync(dynamic employeeId)
        {
            int employee = employeeId;

            var result = from hours in _context.Hour
                         join projects in _context.Project on hours.Id_Project equals projects.Id
                         join clients in _context.Client on projects.Client_Id equals clients.Id
                         where hours.Employee_Id == employee

                         select new ListHour
                         {
                             Id = hours.Id,
                             Project = hours.Project,
                             Date = hours.Date,
                             Start_Time = hours.Start_Time,
                             Stop_Time = hours.Stop_Time,
                             Start_Time_2 = hours.Start_Time_2,
                             Stop_Time_2 = hours.Stop_Time_2,
                             Activies = hours.Activies,
                             Total_Activies_Hours = hours.Total_Activies_Hours,
                             Consultant = hours.Consultant,
                             Creation_Date = hours.Creation_Date,
                             Id_Project = hours.Id_Project,
                             Employee_Id = hours.Employee_Id,
                             Arrival_Time = hours.Arrival_Time,
                             Beginning_Of_The_Break = hours.Beginning_Of_The_Break,
                             End_Of_The_Break = hours.End_Of_The_Break,
                             Exit_Time = hours.Exit_Time,
                             Total_Hours_In_Activity = hours.Total_Hours_In_Activity,
                             Approval = hours.Approval,
                             Approver = hours.Approver,
                             Client = clients.Name
                         };

            return await result
                .OrderBy(x => x.Date)
                .OrderBy(x => x.Arrival_Time)
                .ToListAsync();

        }

        public async Task InsertAsync(Hour obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Hour> FindByIdAsync(int id)
        {
            return await _context.Hour.Include(obj => obj.Project).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Hour.FindAsync(id);
                _context.Hour.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Cant's delete seller because he/sew has sales");
            }
        }

        public async Task UpdateAsync(Hour hour)
        {
            bool hasAny = await _context.Hour.AnyAsync(x => x.Id == hour.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(hour);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {

                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
