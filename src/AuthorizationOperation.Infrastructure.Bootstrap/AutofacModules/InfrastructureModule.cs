using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Infrastructure.Bootstrap.Security;
using AuthorizationOperation.Infrastructure.Core;
using AuthorizationOperation.Infrastructure.EF;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationOperation.Infrastructure.Bootstrap.AutofacModules
{
    public class InfrastructureModule : Module
    {
        public InfrastructureModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationDbContext>()
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
