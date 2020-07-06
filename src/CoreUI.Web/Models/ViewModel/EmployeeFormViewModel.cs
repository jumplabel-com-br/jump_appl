using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class EmployeeFormViewModel
    {
        public Employee Employee { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Access_Level> Access_Level { get; set; }
        public ICollection<Status> TypeReleases { get; set; }
    }
}
