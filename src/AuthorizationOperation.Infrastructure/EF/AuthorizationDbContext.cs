using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.User.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Reflection;

namespace AuthorizationOperation.Infrastructure.EF
{
    public class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext(DbContextOptions options) : base(options) { }

		public DbSet<User> Users { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthorizationDbContext>
    {
        public AuthorizationDbContext CreateDbContext(string[] args)
        {
            if (args == null || args.Length != 1)
                throw new Exception("ConnectionString is not found");

            var connectionString = args[0];
            var builder = new DbContextOptionsBuilder<AuthorizationDbContext>();
            builder.UseMySQL(connectionString);
            return new AuthorizationDbContext(builder.Options);
        }
    }
}
