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

        public async Task<List<Client>> FindAllAsync()
        {
            return await _context.Client.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
