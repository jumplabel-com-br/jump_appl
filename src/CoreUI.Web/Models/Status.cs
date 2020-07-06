using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string NameSelect { get; set; }
        public int User { get; set; }
        public int Active { get; set; }

    }
}
