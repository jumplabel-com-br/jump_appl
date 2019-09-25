using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CoreUI.Web.Models;

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
        public DbSet<CoreUI.Web.Models.Project_team> Project_team { get; set; }


    }
}
