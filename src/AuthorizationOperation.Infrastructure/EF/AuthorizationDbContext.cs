using AuthorizationOperation.Domain.Authorization.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AuthorizationOperation.Infrastructure.EF
{
    public class AuthorizationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySQL("server=localhost;port=3306;user=root;password=Root1234Admin;database=authorization_db");
        }

        public DbSet<Authorization> Authorizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
