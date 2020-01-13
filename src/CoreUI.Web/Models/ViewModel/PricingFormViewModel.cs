using CoreUI.Web.Models.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class PricingFormViewModel
    {
        public Pricing Pricing { get; set; }
        public List<ListPricing> ListPricing { get; set; }
        public PricingDetails PricingDetails { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Employee> Managers { get; set; }
        public ICollection<PricingType> PricingType { get; set; }
        public ICollection<Hiring> Hiring { get; set; }
    }
}
