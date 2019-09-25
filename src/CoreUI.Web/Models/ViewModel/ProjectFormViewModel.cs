using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class ProjectFormViewModel
    {
        public Project Project { get; set; }
        public ICollection<Client> Client { get; set; }
    }
}
