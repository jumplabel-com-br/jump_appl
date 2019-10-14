using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.List
{
    public class ListHour
    {
        public int Id { get; set; }

        [Display(Name = "Projeto")]
        public string Project { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "{0} requerid")]
        public DateTime Date { get; set; }


        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Start_Time { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Stop_Time { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Start_Time_2 { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Stop_Time_2 { get; set; }

        [Display(Name = "Atividades")]
        public string Activies { get; set; }

        [Display(Name = "Total de horas")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        [Required(ErrorMessage = "{0} preencha este campo")]
        public TimeSpan Total_Activies_Hours { get; set; }
        public string Consultant { get; set; }

        [Display(Name = "Data de Criação")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Creation_Date { get; set; }

        [Required(ErrorMessage = "{0} requerid")]
        [Display(Name = "Id Project")]
        public int Id_Project { get; set; }

        public int Employee_Id { get; set; }


        [Display(Name = "Entrada")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Arrival_Time { get; set; }

        [Display(Name = "Pausa")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Beginning_Of_The_Break { get; set; }

        [Display(Name = "Retorno")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime End_Of_The_Break { get; set; }

        [Display(Name = "Saída")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Exit_Time { get; set; }

        [Display(Name = "Total")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Total_Hours_In_Activity { get; set; }

        public int Approval { get; set; }
        public int Approver { get; set; }

        [Display(Name = "Cliente")]
        public string Client { get; set; }
    }
}
