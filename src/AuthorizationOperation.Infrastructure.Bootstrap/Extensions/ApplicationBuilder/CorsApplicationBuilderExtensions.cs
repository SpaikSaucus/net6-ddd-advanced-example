using Microsoft.AspNetCore.Builder;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ApplicationBuilder
{
    public static class CorsApplicationBuilderExtensions
    {
        public static void UseCorsExtension(this IApplicationBuilder app)
        {
            app.UseCors("CorsPolicy");
        }
    }
}
