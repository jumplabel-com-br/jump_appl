using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class OutlaysFormViewModel
    {
        public Outlays Outlays { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Client> Clients { get; set; }
    }
}
