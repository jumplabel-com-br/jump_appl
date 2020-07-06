using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;
using CoreUI.Web.Models.List.API;

namespace CoreUI.Web.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Hour> Hour { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Access_Level> Access_Level { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Project_team> Project_team { get; set; }
        public DbSet<Outlays> Outlays { get; set; }
        public DbSet<Description> Description { get; set; }
        public DbSet<Locality> Locality { get; set; }
        public DbSet<CoreUI.Web.Models.Pricing> Pricing { get; set; }
        public DbSet<CoreUI.Web.Models.Hiring> Hiring { get; set; }
        public DbSet<CoreUI.Web.Models.PricingType> PricingType { get; set; }
        public DbSet<CoreUI.Web.Models.PricingDetails> PricingDetails { get; set; }
        public DbSet<CoreUI.Web.Models.Status> Status { get; set; }
        public DbSet<ListHourAPI> ListHourAPI { get; set; }


    }
}
