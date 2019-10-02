using CoreUI.Web.Models;
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

        public async Task<List<Hour>> FindAllAsync()
        {
            var result = from hour in _context.Hour
                         select new Hour
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
                             Consultant = hour.Consultant.Replace("@jumplabel.com.br", ""),
                             Creation_Date = hour.Creation_Date,
                             Id_Project = hour.Id_Project,
                             Employee_Id = hour.Employee_Id,
                             Arrival_Time = hour.Arrival_Time,
                             Beginning_Of_The_Break = hour.Beginning_Of_The_Break,
                             End_Of_The_Break = hour.End_Of_The_Break,
                             Exit_Time = hour.Exit_Time,
                             Total_Hours_In_Activity = hour.Total_Hours_In_Activity,
                             Approval = hour.Approval,
                             Approver = hour.Approver
                         };

            return await result.OrderBy(x => x.Date).ToListAsync();

        }

        public async Task<List<Hour>> FindAllPerEmployeeAsync(dynamic employeeId)
        {
            int employee = employeeId;

            return await _context.Hour.Where(x => x.Employee_Id == employee).OrderBy(x => x.Date).ToListAsync();

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

        public int TotalMessagesBell()
        {
            return _context.Hour
                .Where(x => x.Approval == 1)
                .Sum(x => x.Approval);
        }
    }
}
