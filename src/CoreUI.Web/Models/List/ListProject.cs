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

        public  int Project_Id { get; set; }
        [Display(Name = "Projeto")]
        public string Project { get; set; }

        public int Client_Id { get; set; }
        [Display(Name = "Cliente")]
        public string Client { get; set; }
        public string Status { get; set; }

        [Display(Name = "Centro de Custo")]
        public long CostCenter { get; set; }


        public int Project_Manager_Id { get; set; }
        public int Manager_Id { get; set; }

        [Display(Name = "Gerente do projeto")]
        public string Project_Manager { get; set; }
        [Display(Name = "Gerente")]
        public string Manager { get; set; }
        public int AcessLevel { get; set; }
        public long Cost_Center_Id { get; set; }
        public int Active { get; set; }
        public int Employee_Id { get; set; }
    }
}
