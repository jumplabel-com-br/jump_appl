using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class DetailsPricing
    {
        public int Id { get; set; }
        public int TypeContract { get; set; }
        public int Hiring_Id { get; set; }
        public string SpecialtyName { get; set; }
        public DateTime HoursMonth { get; set; }
        public DateTime HourConsultant { get; set; }
        public DateTime HourSale { get; set; }
        public double ValueCLTType { get; set; }
        public double VT { get; set; }
        public double Cust { get; set; }
        public int AgeYears { get; set; }

    }
}
