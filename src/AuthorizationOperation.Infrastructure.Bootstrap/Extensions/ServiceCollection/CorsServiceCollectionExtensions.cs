using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class CorsServiceCollectionExtensions
    {
        public static void AddCorsExtension(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
        }
    }
}
