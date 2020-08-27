using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Email
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "Remetente")]
        public string Remetente { get; set; }

        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Display(Name = "Mensagem")]
        public string Mensagem { get; set; }

        [Display(Name = "Email de destino")]
        public string Destino { get; set; }

        [Display(Name = "Assunto")]
        public string Assunto { get; set; }

        [Display(Name = "Empresa")]
        public string Empresa { get; set; }

        [Display(Name = "Tipo de Curso")]
        public string TipoCurso{ get; set; }

        [Display(Name = "Data de envio")]
        public DateTime DataEnvio { get; set; }

        public Email() { }
        public Email(int id, string nome, string remetente, string telefone, string mensagem, string destino, string assunto, string empresa, string  tipoCurso, DateTime dataEnvio) 
        {
            Id = id;
            Nome = nome;
            Remetente = remetente;
            Telefone = telefone;
            Mensagem = mensagem;
            Destino = destino;
            Assunto = assunto;
            Empresa = empresa;
            TipoCurso = tipoCurso;
            DataEnvio = dataEnvio;
        }
    }
}
