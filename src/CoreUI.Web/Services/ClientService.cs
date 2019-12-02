using CoreUI.Web.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class ClientService
    {
        private readonly ApplicationDbContext _context;


        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> FindAllAsync(int? accessLevel, int? employeeId)
        {
            var result = from client in _context.Client
                         select client;

            if (accessLevel == 2)
            {
                result = from client in _context.Client
                  join project in _context.Project on client.Id equals project.Client_Id
                  where project.Project_Manager_Id == employeeId
                  select client;
            }
            
                         
            return await result
                .OrderBy(x => x.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<Client>> FindClientAsync(int? id)
        {
            return await _context.Client
                .Where(x => x.Id == id)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
