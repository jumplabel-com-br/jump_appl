using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Outlays
    {
        public int Id { get; set; }

        [Display(Name = "Funcionário")]
        public int Employee_Id { get; set; }

        [Display(Name = "Projeto")]
        public int Project_Id { get; set; }

        [Display(Name = "Cliente")]
        public int Client_Id { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Número da Nota")]
        public string NoteNumber { get; set; }

        [Display(Name = "Valor da Nota")]
        //[DisplayFormat(DataFormatString = "{0:F2}")]
        public double NoteValue { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Arquivo")]
        public string File { get; set; }
        public int Status { get; set; }

    }
}
