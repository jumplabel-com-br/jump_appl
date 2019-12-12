using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Pricing
    {
        public int Id { get; set; }
        public int TypePricing { get; set; }
        public int Client_Id { get; set; }
        public string Allocation { get; set; }
        public string AccountExecutive { get; set; }
        public int NumberProposal { get; set; }
        public int AllocationManager_Id { get; set; }
        public int Administrator_Id { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TimeBetweenInitialAndEndDate { get; set; }
        public int Risk { get; set; }
    }
}
