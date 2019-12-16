using CoreUI.Web.Models;
using CoreUI.Web.Models.List;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class PricingService
    {
        private readonly ApplicationDbContext _context;


        public PricingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ListPricing>> FindAllAsync()
        {
            var result = from pricing in _context.Pricing
                         join typePricint in _context.TypePricing on pricing.TypePricing equals typePricint.Id

                         join client in _context.Client on pricing.Client_Id equals client.Id
                            into clients
                         from client in clients.DefaultIfEmpty()
                         join employee in _context.Employee on pricing.AllocationManager_Id equals employee.Id
                             into employees
                         from employee in employees.DefaultIfEmpty()

                         select new ListPricing
                         {
                             Id = pricing.Id,
                             TypePricing = pricing.TypePricing,
                             Client_Id = pricing.Client_Id,
                             Allocation = pricing.Allocation,
                             AccountExecutive = pricing.AccountExecutive,
                             NumberProposal = pricing.NumberProposal,
                             AllocationManager_Id = pricing.AllocationManager_Id,
                             Administrator_Id = pricing.Administrator_Id,
                             InitialDate = pricing.InitialDate,
                             EndDate = pricing.EndDate,
                             TimeBetweenInitialAndEndDate = pricing.TimeBetweenInitialAndEndDate,
                             Risk = pricing.Risk
                         };

            return await result
                .ToListAsync();
        }
    }
}
