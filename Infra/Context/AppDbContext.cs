using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        
    }

    public class AppFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            string mySqlConnection = @"host=localhost; Port=3306; Database=db;username=root;password=mySql2019;";
            optionsBuilder.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection));

            return new AppDbContext(optionsBuilder.Options);
        }
    }

}
