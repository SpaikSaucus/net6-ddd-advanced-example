using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Infrastructure.Bootstrap.Security;
using AuthorizationOperation.Infrastructure.Core;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace AuthorizationOperation.Infrastructure.Bootstrap.AutofacModules
{
    public class InfrastructureModule : Module
    {
        private readonly IConfiguration configuration;

        public InfrastructureModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
             .As<IUnitOfWork>()
             .InstancePerLifetimeScope();

            builder.RegisterType<JwtProvider>()
             .As<ITokenProvider>()
             .InstancePerLifetimeScope();
        }
    }
}
