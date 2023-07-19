using ConsoleTestApp.EfCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp.EfCore
{
    internal class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=TestApplicationDb;Trusted_Connection=True;");
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ApplicationUserPhoto> Photos { get; set; }

    }
}
