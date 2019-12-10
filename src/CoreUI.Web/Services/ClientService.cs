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
            /*
            if (accessLevel == 2)
            {
                result = from client in _context.Client
                  join project in _context.Project on client.Id equals project.Client_Id
                  join projectTeam
                  where project.Project_Manager_Id == employeeId
                  select client;
            }
            */

            if (accessLevel == 2 || accessLevel == 3)
            {
                result = from client in _context.Client
                         join project in _context.Project on client.Id equals project.Client_Id
                         join projectTeam in _context.Project_team on project.Id equals projectTeam.Project_Id
                         where projectTeam.Employee_Id == employeeId
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
