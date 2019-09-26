using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class ProjectTeamFormViewModel
    {
        public Project_team Project_team { get; set; }
        public Employee Employees { get; set; }
        public Project Projects { get; set; }

        public ICollection<Employee> Employee { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}
