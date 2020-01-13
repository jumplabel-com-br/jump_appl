using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class PricingDetails
    {
        public int Id { get; set; }
        public int TypeContract { get; set; }
        public int Pricing_Id { get; set; }
        public string SpecialtyName { get; set; }
        public int HoursMonth { get; set; }
        public double HourConsultant { get; set; }
        public double HourSale { get; set; }
        public double ValueCLTType { get; set; }
        public double VT { get; set; }
        public double Cust { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateBirth { get; set; }
        public int AgeYears { get; set; }
    }
}
