using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.User.Models;
using AuthorizationOperation.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Setup
{
	public class AuthorizationMockDbContext : AuthorizationDbContext
	{
		public AuthorizationMockDbContext(DbContextOptions options) : base(options)
		{
			this.Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var user in UsersMock.Get)
			{
				modelBuilder.Entity<User>().HasData(
					new User(user.UUID, user.UserName, user.Password, user.Email)
				);
			}

			foreach (var authorization in AuthorizationsMock.Get)
			{
				modelBuilder.Entity<Authorization>().HasData(
					new Authorization()
					{
						Id = authorization.Id,
						UUID = authorization.UUID,
						Customer = authorization.Customer,
						StatusId = authorization.StatusId,
						Created = authorization.Created
					}
				);
			}

			base.OnModelCreating(modelBuilder);
		}
	}
}
