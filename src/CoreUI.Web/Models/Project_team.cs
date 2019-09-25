using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Project_team
    {
        public int Id { get; set; }

        public Project Project { get; set; }
        public int Project_Id { get; set; }

        public Employee Employee { get; set; }
        public int Employee_Id { get; set; }

        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
    }
}
