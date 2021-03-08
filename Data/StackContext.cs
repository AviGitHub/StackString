using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using WebStack.Data.Models;
using Microsoft.Extensions.Configuration;

namespace WebStack.Data
{
    public class StackContext : DbContext
    {
        public DbSet<StackString> StackStrings { get; set; }

        public DbSet<StackConfiguration> Configurations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // TODO: replace with connection string from appsettings
            optionsBuilder.UseSqlServer(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}