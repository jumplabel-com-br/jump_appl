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

        [Display(Name = "Project")]
        public string Project_Name { get; set; }

        public int Client_Id { get; set; }
        public long Cost_Center_Id { get; set; }
        public int Active { get; set; }

        public Project()
        {

        }

        public Project(int id, string project_Name, int client_Id, long cost_Center_Id, int active)
        {
            Id = id;
            Project_Name = project_Name;
            Client_Id = client_Id;
            Cost_Center_Id = cost_Center_Id;
            Active = active;
        }
    }
}
