﻿using CoreUI.Web.Models.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models.ViewModel
{
    public class OutlaysFormViewModel
    {
        public Outlays Outlays { get; set; }
        public List<ListOutlays> Outlay { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Project> Projects { get; set; }
        public List<ListProject> ListProjects { get; set; }
        public ICollection<Client> Clients { get; set; }
        public ICollection<Status> Status { get; set; }
    }
}
