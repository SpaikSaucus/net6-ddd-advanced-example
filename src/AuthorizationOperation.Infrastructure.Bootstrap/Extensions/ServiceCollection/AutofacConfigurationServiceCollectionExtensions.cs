using Autofac;
using Microsoft.Extensions.Configuration;
using AuthorizationOperation.Infrastructure.Bootstrap.AutofacModules;
using AuthorizationOperation.Infrastructure.Bootstrap.Security;
using AuthorizationOperation.Infrastructure.Services;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class AutofacConfigurationServiceCollectionExtensions
    {
        public static void AddAutofacExtension(this ContainerBuilder builder, IConfiguration configuration)
        {
            _ = builder.RegisterType<HttpContextCurrentUserAccessor>()
                .As<ICurrentUserAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterModule(new InfrastructureModule(configuration));
            builder.RegisterModule(new MediatorModule(configuration.GetValue("CommandLoggingEnabled", false)));                   
        }
    }
}
