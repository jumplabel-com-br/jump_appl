using CoreUI.Web.Models.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class ProjectFormViewModel
    {
        public Project Project { get; set; }
        public List<ListProject> ListProject { get; set; }
        public ICollection<Client> Client { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Employee> Employee { get; set; }
    }
}
