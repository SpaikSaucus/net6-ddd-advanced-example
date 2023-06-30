using Refit;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class HttpClientServiceCollectionExtensions
    {
        public static void AddHttpClientFactoryExtension(this IServiceCollection services, IConfiguration configuration)
        {
            //services
            //   .AddRefitClient<IExternalUserService>()
            //   .ConfigureHttpClient((_, client) =>
            //   {
            //       client.BaseAddress = new Uri(configuration.GetSection("AppSettings:ClientServiceUrl").Value);
            //   });
        }
    }    
}
