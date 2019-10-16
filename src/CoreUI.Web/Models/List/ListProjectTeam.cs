using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class ListProjectTeam
    {
        public int Id { get; set; }

        [Display(Name = "Cliente")]
        public string Client { get; set; }

        [Display(Name = "Funcionário")]
        public string Employee { get; set; }
        public int EmployeeId { get; set; }

        [Display(Name = "Projeto")]
        public string Project { get; set; }
        public int ProjectId { get; set; }

        [Display(Name = "Data de início")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Start { get; set; }

        [Display(Name = "Data de término")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime End { get; set; }
    }
}
