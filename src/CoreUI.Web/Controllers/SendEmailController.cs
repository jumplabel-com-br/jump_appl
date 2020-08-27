using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreUI.Web.Services;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using CoreUI.Web.Models;
using CoreUI.Web.Services;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CoreUI.Web.Controllers
{
    public class SendEmailController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public SendEmailController(IEmailSender emailSender, ApplicationDbContext context, IHostingEnvironment env, IConfiguration config)
        {
            _emailSender = emailSender;
            _context = context;
            _config = config;
        }


        public async Task EnviaEmail(Email email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EnvioEmail(email.Destino, email.Assunto, email.Mensagem).GetAwaiter();
                }
                catch (Exception)
                {

                }
            }
        }

        public async Task EnvioEmail(string email, string assunto, string mensagem)
        {
            try
            {
                //email destino, assunto do email, mensagem a enviar
                await _emailSender.SendEmailAsync(email, assunto, mensagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<string> forgotPassword(string email, string senha)
        {
            try
            {
                if (_context.Employee.Count(elem => elem.Email == email) == 0)
                {
                    return "Não há um usuário com esta registro de email";
                }

                string query = "update Employee set password = '" + senha + "', change_password = 1 where Email = '" + email + "'";
                string message = "Sua nova senha para acesso: " + senha;
                ExecuteQuery(query);

                await EnvioEmail(email, "Nova senha", message);

                return "Foi enviado um email com a nova senha de acesso.";
            }
            catch (Exception e)
            {

                return e.Message;
            }

        }

        public async Task<string> ProjectTeamAlert(int id, string projeto, string dt_inicial, string dt_final)
        {
            try
            {
                string message = "O projeto " + projeto + " foi liberado entre a data " + dt_inicial + " até a data " + dt_final;
                string email = ReturnEmail(id);

                if (_context.Employee.Count(elem => elem.Email == email) == 0)
                {
                    return "Não há um usuário com esta registro de email";
                }

                await EnvioEmail(email, "Projeto Liberado", message);

                return "Foi enviado um email sobre este projeto.";
            }
            catch (Exception e)
            {

                return e.Message;
            }

        }

        public void ExecuteQuery(string query)
        {
            string conn = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");
            MySqlConnection connection = new MySqlConnection(conn);
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public string ReturnEmail(int id)
        {
            string query = "SELECT * FROM Employee where id = '" + id + "' and active = 1";
            string connString = _config.GetValue<string>("ConnectionStrings:ApplicationDbContext");

            MySqlConnection connection = new MySqlConnection(connString);
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataAdapter da = new MySqlDataAdapter();

            da.SelectCommand = command;

            DataTable dt = new DataTable();

            da.Fill(dt);

            string Email = dt.Rows[0]["Email"].ToString();
            return Email;
        }
    }
}