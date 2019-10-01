using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Project_team
    {
        public int Id { get; set; }

        [Display(Name = "Projeto")]
        public int Project_Id { get; set; }

        [Display(Name = "Funcionário")]
        public int Employee_Id { get; set; }

        [Display(Name = "Data de início")]
        [DataType(DataType.Date)]
        public DateTime Start_Date { get; set; }

        [Display(Name = "Date de término")]
        [DataType(DataType.Date)]
        public DateTime End_Date { get; set; }

       
    }
}
