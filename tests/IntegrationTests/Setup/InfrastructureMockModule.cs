using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Infrastructure.Bootstrap.Security;
using AuthorizationOperation.Infrastructure.Core;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Setup
{
	public class InfrastructureMockModule : Module
	{
		public InfrastructureMockModule()
		{
		}

		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<AuthorizationMockDbContext>()
			 .As<DbContext>()
			 .InstancePerLifetimeScope();

			builder.RegisterType<UnitOfWork>()
			 .As<IUnitOfWork>()
			 .InstancePerLifetimeScope();

			builder.RegisterType<JwtProvider>()
			 .As<ITokenProvider>()
			 .InstancePerLifetimeScope();
		}
	}
}
