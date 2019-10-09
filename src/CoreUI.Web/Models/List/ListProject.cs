using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.List
{
    public class ListProject
    {
        public int Id { get; set; }
        [Display(Name = "Projeto")]
        public string Project { get; set; }

        [Display(Name = "Cliente")]
        public string Client { get; set; }
        public string Status { get; set; }

        [Display(Name = "Centro de Custo")]
        public long CostCenter { get; set; }
    }
}
