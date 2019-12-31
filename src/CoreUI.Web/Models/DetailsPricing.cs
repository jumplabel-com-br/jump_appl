using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class DetailsPricing
    {
        public int Id { get; set; }
        public int TypeContract { get; set; }
        public int Pricing_Id { get; set; }
        public string SpecialtyName { get; set; }
        public int HoursMonth { get; set; }
        public int HourConsultant { get; set; }
        public int HourSale { get; set; }
        public double ValueCLTType { get; set; }
        public double VT { get; set; }
        public double Cust { get; set; }
        public int AgeYears { get; set; }

    }
}
