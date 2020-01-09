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

        public async Task<List<ListPricing>> FindAllAsync(int? tipoPrecificacao, int? clients, int? executivoConta, int? alocacaoGerente, int? responsavel, DateTime? dtInicial, DateTime? dtFinal)
        {
            var result = from pricing in _context.Pricing
                         join typePricing in _context.TypePricing on pricing.TypePricing equals typePricing.Id
                            into typesPricings
                         from typePricing in typesPricings.DefaultIfEmpty()

                         join client in _context.Client on pricing.Client_Id equals client.Id
                            into clientes
                         from client in clientes.DefaultIfEmpty()

                         join executiveAccounts in _context.Employee on pricing.AccountExecutive_Id equals executiveAccounts.Id
                             into executiveAccount
                         from executiveAccounts in executiveAccount.DefaultIfEmpty()

                         join employeeManager in _context.Employee on pricing.Administrator_Id equals employeeManager.Id
                             into employeesManager
                         from employeeManager in employeesManager.DefaultIfEmpty()


                         join employee in _context.Employee on pricing.AllocationManager_Id equals employee.Id
                             into employees
                         from employee in employees.DefaultIfEmpty()

                         select new ListPricing
                         {
                             Id = pricing.Id,
                             TypePricing = pricing.TypePricing,
                             TypesPricing = typePricing.Name,
                             Client_Id = client.Id,
                             Cliente = client.Name,
                             Allocation = pricing.Allocation,
                             AccountExecutive_Id = executiveAccounts.Id,
                             AccountExecutive = executiveAccounts.Name,
                             NumberProposal = pricing.NumberProposal,
                             AllocationManager_Id = employee.Id,
                             AllocationManager = employee.Name,
                             Administrator_Id = employeeManager.Id,
                             Administrator = employeeManager.Name,
                             InitialDate = pricing.InitialDate,
                             EndDate = pricing.EndDate,
                             TimeBetweenInitialAndEndDate = pricing.TimeBetweenInitialAndEndDate,
                             Risk = pricing.Risk
                         };


            if (tipoPrecificacao.HasValue)
            {
                result = result.Where(x => x.TypePricing == tipoPrecificacao);
            }
            if (clients.HasValue)
            {
                result = result.Where(x => x.Client_Id == clients);
            }

            if (executivoConta.HasValue)
            {
                result = result.Where(x => x.AccountExecutive_Id == executivoConta);
            }

            if (alocacaoGerente.HasValue)
            {
                result = result.Where(x => x.AllocationManager_Id == alocacaoGerente);
            }

            if (responsavel.HasValue)
            {
                result = result.Where(x => x.Administrator_Id == responsavel);
            }

            if (dtInicial.HasValue)
            {
                result = result.Where(x => x.InitialDate >= dtInicial.Value);
            }

            if (dtFinal.HasValue)
            {
                result = result.Where(x => x.EndDate <= dtFinal.Value);
            }

            return await result
                .ToListAsync();
        }

        public async Task<DetailsPricing> DetailsPricing(int? id = null)
        {
            var result = from detailsPricing in _context.DetailsPricing
                         select new DetailsPricing
                         {
                             Id = detailsPricing.Id,
                             TypeContract = detailsPricing.TypeContract == null ? 0 : detailsPricing.TypeContract,
                             Pricing_Id = detailsPricing.Pricing_Id == null ? 0 : detailsPricing.Pricing_Id,
                             SpecialtyName = detailsPricing.SpecialtyName == null ? "" : detailsPricing.SpecialtyName,
                             HoursMonth = detailsPricing.HoursMonth == null ? 0 : detailsPricing.HoursMonth,
                             HourConsultant = detailsPricing.HourConsultant == null ? 0 : detailsPricing.HourConsultant,
                             HourSale = detailsPricing.HourSale == null ? 0 : detailsPricing.HourSale,
                             ValueCLTType = detailsPricing.ValueCLTType == null ? 0 : detailsPricing.ValueCLTType,
                             VT = detailsPricing.VT == null ? 0 : detailsPricing.VT,
                             Cust = detailsPricing.Cust == null ? 0 : detailsPricing.Cust,
                             AgeYears = detailsPricing == null ? 0 : detailsPricing.AgeYears
                         };

            if (id.HasValue)
            {
                result = result.Where(x => x.Id == id);
            }

            return await result.FirstAsync();
        }
    }
}
