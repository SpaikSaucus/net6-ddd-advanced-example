using Microsoft.AspNetCore.Builder;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ApplicationBuilder
{
    public static class HealthChecksApplicationBuilderExtensions
    {
        public static void UseHealthChecksExtension(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health");
        }
    }
}
