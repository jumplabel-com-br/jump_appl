using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Display(Name = "Projeto")]
        public string Project_Name { get; set; }

        [Display(Name = "Cliente")]
        public int Client_Id { get; set; }
        public long Cost_Center_Id { get; set; }
        
        [Display(Name = "Status")]
        public int Active { get; set; }

        [Display(Name = "Gerente de projeto")]
        public int Project_Manager_Id { get; set; }

        [Display(Name = "Gerente")]
        public int Manager_Id { get; set; }

        public Project()
        {

        }

        public Project(int id, string project_Name, int client_Id, long cost_Center_Id, int active, int project_Manager_Id, int manager_Id)
        {

            Id = id;
            Project_Name = project_Name;
            Client_Id = client_Id;
            Cost_Center_Id = cost_Center_Id;
            Active = active;
            Project_Manager_Id = project_Manager_Id;
            Manager_Id = manager_Id;

        }
    }
}
