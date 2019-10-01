using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
