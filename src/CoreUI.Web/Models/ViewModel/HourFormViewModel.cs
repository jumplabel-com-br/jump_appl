using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class HourFormViewModel
    {
        public Hour Hour { get; set; }
        //public Employee Employee { get; set; }
        public Project_team Project_team { get; set; }
        public Employee Employee { get; set; }

        public ICollection<Client> Clients { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Employee> Employees { get; set; }

    }
}
