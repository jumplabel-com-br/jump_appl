using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Hour
    {
        public int Id { get; set; }

        public string Project { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "{0} requerid")]
        public DateTime Date { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Start_Time { get; set; }

        [Display(Name = "Stop Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Stop_Time { get; set; }

        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Start_Time_2 { get; set; }

        [Display(Name = "Stop Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public TimeSpan Stop_Time_2 { get; set; }

        [Display(Name = "Activities")]
        public string Activies { get; set; }

        [Display(Name = "Total Hours")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        [Required(ErrorMessage = "{0} preencha este campo")]
        public TimeSpan Total_Activies_Hours { get; set; }
        public string Consultant { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Creation_Date { get; set; }

        [Required(ErrorMessage = "{0} requerid")]
        [Display(Name = "Id Project")]
        public int Id_Project { get; set; }

        public int Employee_Id { get; set; }


        [Display(Name = "Arrival Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Arrival_Time { get; set; }

        [Display(Name = "Beginnig Of The Break")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Beginning_Of_The_Break { get; set; }

        [Display(Name = "End Of The Break")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime End_Of_The_Break { get; set; }

        [Display(Name = "Exit Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Exit_Time { get; set; }

        [Display(Name = "Total Hours In Activity")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime Total_Hours_In_Activity { get; set; }
        


        public Hour()
        {

        }

        public Hour(int id, string project, DateTime date, TimeSpan start_Time, TimeSpan stop_Time, TimeSpan start_Time_2, TimeSpan stop_Time_2, string activies, TimeSpan total_Activies_Hours, string consultant, DateTime creation_Date, int id_Project, int employee_Id, DateTime arrival_Time, DateTime beginning_Of_The_Break, DateTime end_Of_The_Break, DateTime exit_Time, DateTime total_hours_in_activity)
        {
            Id = id;
            Project = project;
            Date = date;
            Start_Time = start_Time;
            Stop_Time = stop_Time;
            Start_Time_2 = start_Time_2;
            Stop_Time_2 = stop_Time_2;
            Activies = activies;
            Total_Activies_Hours = total_Activies_Hours;
            Consultant = consultant;
            Creation_Date = creation_Date;
            Id_Project = id_Project;
            Employee_Id = employee_Id;
            Arrival_Time = arrival_Time;
            Beginning_Of_The_Break = beginning_Of_The_Break;
            End_Of_The_Break = end_Of_The_Break;
            Exit_Time = exit_Time;
            Total_Hours_In_Activity = total_hours_in_activity;
        }
    }
}
