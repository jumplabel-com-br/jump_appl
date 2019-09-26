using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Project_team
    {
        public int Id { get; set; }

        public int Project_Id { get; set; }
        public int Employee_Id { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime Start_Date { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime End_Date { get; set; }

       
    }
}
