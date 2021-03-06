﻿using CoreUI.Web.Models;
using CoreUI.Web.Models.List;
using CoreUI.Web.Models.List.API;
using CoreUI.Web.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreUI.Web.Services
{
    public class HourService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public HourService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public async Task<List<Status>> FindStatus(string nameSelect)
        {
            var result = from status in _context.Status
                         where status.NameSelect == nameSelect
                         select status;

            return await result
                .OrderBy(x => x.NameSelect)
                .ToListAsync();
        }

        public dynamic APIHours(string email, string senha, DateTime? dtInicio, DateTime? dtFim, string statusDescription, string defaultVerification)
        {
            string[] ElementoVetor = new string[1] { "Acesso inválido" };

            if (defaultVerification != "0")
            {
                defaultVerification = "1";
            }

            string queryString = "select Email from dev_jump.Employee where email = '" + email + "' and password = '" + senha + "' and access_LevelId in (1,5)";
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(queryString, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = command;

            DataTable dt = new DataTable();

            da.Fill(dt);
            object id = dt;

            string NameEmployee = dt.Rows.Count != 0 ? dt.Rows[0]["Email"].ToString() : null;


            if ((NameEmployee == null && defaultVerification == "1") || defaultVerification == null)
            {
                return ElementoVetor;
            }


            if (defaultVerification == "0")
            {
                var result = from horas in _context.Hour
                             join projetos in _context.Project on horas.Id_Project equals projetos.Id
                             join clientes in _context.Client on projetos.Client_Id equals clientes.Id
                             join funcionarios in _context.Employee on horas.Employee_Id equals funcionarios.Id
                             join status in _context.Status on horas.Approval equals status.Id
                             orderby horas.Start_Time ascending

                             select new ListHour
                             {
                                 Id = horas.Id,
                                 Project = horas.Project,
                                 Date = horas.Date,
                                 Start_Time = horas.Start_Time,
                                 Stop_Time = horas.Stop_Time,
                                 Start_Time_2 = horas.Start_Time_2,
                                 Stop_Time_2 = horas.Stop_Time_2,
                                 Activies = horas.Activies,
                                 Total_Activies_Hours = horas.Total_Activies_Hours,
                                 Consultant = funcionarios.Email.Replace("@jumplabel.com.br", ""),
                                 Creation_Date = horas.Creation_Date,
                                 Id_Project = horas.Id_Project,
                                 Employee_Id = horas.Employee_Id,
                                 Arrival_Time = horas.Arrival_Time,
                                 Beginning_Of_The_Break = horas.Beginning_Of_The_Break,
                                 End_Of_The_Break = horas.End_Of_The_Break,
                                 Exit_Time = horas.Exit_Time,
                                 Total_Hours_In_Activity = horas.Total_Hours_In_Activity,
                                 Approval = horas.Approval,
                                 Approver = horas.Approver,
                                 Id_Client = clientes.Id,
                                 Client = clientes.Name,
                                 Description = horas.Description,
                                 Billing = horas.Billing,
                                 Status = status.Description
                             };
                if (dtInicio.HasValue)
                {
                    result = result.Where(x => x.Date >= dtInicio);
                }

                if (dtFim.HasValue)
                {
                    result = result.Where(x => x.Date <= dtFim);
                }

                if (statusDescription != null)
                {
                    result = result.Where(x => x.Status.ToLower() == statusDescription.ToLower());
                }

                return result.OrderBy(x => x.Start_Time);
            }
            else
            {
                var result = from horas in _context.Hour
                             join projetos in _context.Project on horas.Id_Project equals projetos.Id
                             join clientes in _context.Client on projetos.Client_Id equals clientes.Id
                             join funcionarios in _context.Employee on horas.Employee_Id equals funcionarios.Id
                             join status in _context.Status on horas.Approval equals status.Id
                             orderby horas.Start_Time ascending

                             select new ListHourAPI
                             {
                                 Id = horas.Id,
                                 Projeto = horas.Project.Replace("\n", ""),
                                 Clientes = clientes.Name.Replace("\n", ""),
                                 Data = horas.Date.ToString("dd/MM/yyyy"),
                                 Date = horas.Date,
                                 HoraInicio = horas.Start_Time,
                                 Pausa = horas.Stop_Time,
                                 Retorno = horas.Start_Time_2,
                                 HoraFim = horas.Stop_Time_2,
                                 TotalHoras = horas.Total_Activies_Hours,
                                 Atividades = horas.Activies.Replace("\n", ""),
                                 Consultor = funcionarios.Email.Replace("@jumplabel.com.br", ""),
                                 Cobranca = horas.Billing == 1 ? "SIM" : "NÂO",
                                 Status = status.Description
                             };

                if (dtInicio.HasValue)
                {
                    result = result.Where(x => x.Date >= dtInicio);
                }

                if (dtFim.HasValue)
                {
                    result = result.Where(x => x.Date <= dtFim);
                }

                if (statusDescription != null)
                {
                    result = result.Where(x => x.Status.ToLower() == statusDescription.ToLower());
                }

                return result.OrderBy(x => x.HoraInicio);
            }




        }

        public async Task<List<ListHour>> FindAllAsync(int? month, int? year)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var result = from horas in _context.Hour
                         join projetos in _context.Project on horas.Id_Project equals projetos.Id
                         join clientes in _context.Client on projetos.Client_Id equals clientes.Id
                         join funcionarios in _context.Employee on horas.Employee_Id equals funcionarios.Id
                         join status in _context.Status on horas.Approval equals status.Id
                         //where hour.Date.Month == month && hour.Date.Year == year
                         //join projectTeam in _context.Project_team on projects.Id equals projectTeam.Project_Id
                         //where projects.Active == 1 //&& employees.Active == 1
                         orderby horas.Start_Time ascending

                         select new ListHour
                         {
                             Id = horas.Id,
                             Project = horas.Project,
                             Date = horas.Date,
                             Start_Time = horas.Start_Time,
                             Stop_Time = horas.Stop_Time,
                             Start_Time_2 = horas.Start_Time_2,
                             Stop_Time_2 = horas.Stop_Time_2,
                             Activies = horas.Activies,
                             Total_Activies_Hours = horas.Total_Activies_Hours,
                             Consultant = funcionarios.Email.Replace("@jumplabel.com.br", ""),
                             Creation_Date = horas.Creation_Date,
                             Id_Project = horas.Id_Project,
                             Employee_Id = horas.Employee_Id,
                             Arrival_Time = horas.Arrival_Time,
                             Beginning_Of_The_Break = horas.Beginning_Of_The_Break,
                             End_Of_The_Break = horas.End_Of_The_Break,
                             Exit_Time = horas.Exit_Time,
                             Total_Hours_In_Activity = horas.Total_Hours_In_Activity,
                             Approval = horas.Approval,
                             Approver = horas.Approver,
                             Id_Client = clientes.Id,
                             Client = clientes.Name,
                             Description = horas.Description,
                             Billing = horas.Billing,
                             Status = status.Description
                         };

            if (month.HasValue)
            {
                result = result.Where(x => x.Date.Month == month);
            }

            if (year.HasValue)
            {
                result = result.Where(x => x.Date.Year == year);
            }

            return await result
                //.OrderBy(x => x.Client)
                //.OrderBy(x => x.Project)
                //.OrderBy(x => x.Consultant)
                .OrderBy(x => x.Date)
                //.OrderBy(x b=> x.Start_Time)
                .ToListAsync();

        }

        public async Task<List<ListHour>> FindAllAsync(int? billing, int? approval, int? description, int? clients, int? projects, int? employees, int? month, int? year, int? empId, int? accessLevel)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var result = from horas in _context.Hour
                         join projetos in _context.Project on horas.Id_Project equals projetos.Id
                         join clientes in _context.Client on projetos.Client_Id equals clientes.Id
                         join funcionarios in _context.Employee on horas.Employee_Id equals funcionarios.Id
                         join status in _context.Status on horas.Approval equals status.Id
                         join Descriptions in _context.Description on horas.Description equals Descriptions.Id
                           into Description
                         from descriptions in Description.DefaultIfEmpty()
                         join locality in _context.Locality on horas.LocalityId equals locality.Id
                            into localities
                         from locality in localities.DefaultIfEmpty()
                             //where hour.Date.Month == month && hour.Date.Year == year
                             //join projectTeam in _context.Project_team on projects.Id equals projectTeam.Project_Id
                             //where projects.Active == 1 //&& employees.Active == 1
                         /*where horas.Billing == 1*/
                         orderby horas.Start_Time ascending

                         select new ListHour
                         {
                             Id = horas.Id,
                             Project = horas.Project,
                             Date = horas.Date,
                             Start_Time = horas.Start_Time,
                             Stop_Time = horas.Stop_Time,
                             Start_Time_2 = horas.Start_Time_2,
                             Stop_Time_2 = horas.Stop_Time_2,
                             Activies = horas.Activies,
                             Total_Activies_Hours = horas.Total_Activies_Hours,
                             Consultant = funcionarios.Email.Replace("@jumplabel.com.br", ""),
                             Creation_Date = horas.Creation_Date,
                             Id_Project = horas.Id_Project,
                             Employee_Id = horas.Employee_Id,
                             Arrival_Time = horas.Arrival_Time,
                             Beginning_Of_The_Break = horas.Beginning_Of_The_Break,
                             End_Of_The_Break = horas.End_Of_The_Break,
                             Exit_Time = horas.Exit_Time,
                             Total_Hours_In_Activity = horas.Total_Hours_In_Activity,
                             Approval = horas.Approval,
                             Approver = horas.Approver,
                             Id_Client = clientes.Id,
                             Client = clientes.Name,
                             Description = horas.Description,
                             Description_Name = descriptions.Name,
                             Billing = horas.Billing,
                             LocalityId = locality.Id == null ? 1 : locality.Id,
                             Project_Manager_Id = projetos.Project_Manager_Id,
                             Status = status.Description
                         };

            if (accessLevel == 2)
            {
                result = result.Where(x => x.Project_Manager_Id == empId);
            }

            if (month.HasValue)
            {
                result = result.Where(x => x.Date.Month == month);
            }

            if (year.HasValue)
            {
                result = result.Where(x => x.Date.Year == year);
            }

            if (billing.HasValue)
            {
                result = result.Where(x => x.Billing == billing);
            }

            if (approval.HasValue)
            {
                result = result.Where(x => x.Approval == approval);
            }

            if (description.HasValue)
            {
                result = result.Where(x => x.Description == description);
            }

            if (clients.HasValue)
            {
                result = result.Where(x => x.Id_Client == clients);
            }

            if (projects.HasValue)
            {
                result = result.Where(x => x.Id_Project == projects);
            }

            if (employees.HasValue)
            {
                result = result.Where(x => x.Employee_Id == employees);
            }


            return await result
                //.OrderBy(x => x.Client)
                //.OrderBy(x => x.Project)
                //.OrderBy(x => x.Consultant)
                .OrderBy(x => x.Date)
                //.OrderBy(x b=> x.Start_Time)
                .ToListAsync();

        }

        public async Task<List<ListHour>> FindAllPerEmployeeAsync(dynamic employeeId, int? billing, int? approval, int? description, int? clients, int? projects, int? month, int? year)
        {
            int employee = employeeId;

            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            if (!month.HasValue)
            {
                month = DateTime.Now.Month;
            }

            var result = from hours in _context.Hour
                         join projetos in _context.Project on hours.Id_Project equals projetos.Id
                         join clientes in _context.Client on projetos.Client_Id equals clientes.Id
                         join status in _context.Status on hours.Approval equals status.Id
                         join Descriptions in _context.Description on hours.Description equals Descriptions.Id
                            into Description
                         from descriptions in Description.DefaultIfEmpty()
                         join locality in _context.Locality on hours.LocalityId equals locality.Id
                            into localities
                         from locality in localities.DefaultIfEmpty()

                         where hours.Employee_Id == employee
                         orderby hours.Start_Time ascending
                         //orderby hours.Start_Time ascending

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
                             Id_Client = clientes.Id,
                             Client = clientes.Name,
                             Description = hours.Description,
                             Description_Name = descriptions.Name,
                             Billing = hours.Billing,
                             File = hours.File == null ? "Sem Documento" : hours.File,
                             LocalityId = locality.Id == null ? 1 : locality.Id,
                             Status = status.Description

                         };

            if (month.HasValue)
            {
                result = result.Where(x => x.Date.Month == month);
            }

            if (year.HasValue)
            {
                result = result.Where(x => x.Date.Year == year);
            }

            if (billing.HasValue)
            {
                result = result.Where(x => x.Billing == billing);
            }

            if (approval.HasValue)
            {
                result = result.Where(x => x.Approval == approval);
            }

            if (description.HasValue)
            {
                result = result.Where(x => x.Description == description);
            }

            if (clients.HasValue)
            {
                result = result.Where(x => x.Id_Client == clients);
            }

            if (projects.HasValue)
            {
                result = result.Where(x => x.Id_Project == projects);
            }

            return await result
                .OrderBy(x => x.Date)
                //.OrderBy(x => x.Arrival_Time)
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
