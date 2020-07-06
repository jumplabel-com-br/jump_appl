using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.List.API
{
    public class ListHourAPI
    {
        public int Id { get; set; }
        public string Projeto { get; set; }
        public string Clientes { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan Pausa { get; set; }
        public TimeSpan Retorno { get; set; }
        public TimeSpan HoraFim { get; set; }
        public string Atividades { get; set; }
        public TimeSpan TotalHoras { get; set; }
        public string Consultor { get; set; }
        public string Senha { get; set; }
        public string Cobranca { get; set; }
        public string Status { get; set; }
    }
}
