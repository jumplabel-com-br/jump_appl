using CoreUI.Web.Models;
using CoreUI.Web.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Hour>> FindAllAsync(dynamic employeeId)
        {
            int employee = employeeId;
            return await _context.Hour.Where(x => x.Employee_Id == employee).ToListAsync();
            /*
            var result = from obj in _context.Hour select obj;

            int employee = employeeId;

            result = result.Where(x => x.Employee_Id == employee);

            return await result
                .ToListAsync();*/
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
            catch (DbUpdateException e)
            {
                throw new IntegrityException("Cant's delete seller because he/sew has sales");
            }
        }

        public async Task UpdateAsync(Hour obj)
        {
            bool hasAny = await _context.Hour.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {

                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
