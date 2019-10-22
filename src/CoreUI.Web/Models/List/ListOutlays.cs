using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.List
{
    public class ListOutlays
    {
        public int Id { get; set; }

        [Display(Name = "Funcionário")]
        public string Employee { get; set; }

        [Display(Name = "Projeto")]
        public string Project { get; set; }

        [Display(Name = "Cliente")]
        public string Client { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Número da Nota")]
        public string NoteNumber { get; set; }

        [Display(Name = "Valor da Nota")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double NoteValue { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Arquivo")]
        public string File { get; set; }
    }
}
