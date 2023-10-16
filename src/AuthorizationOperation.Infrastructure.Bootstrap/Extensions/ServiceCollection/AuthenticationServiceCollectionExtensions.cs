namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class AuthenticationServiceCollectionExtensions
    {
        public static void AddAuthenticationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
#warning ValidateIssuer and ValidateAudience, change to true in production.
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["AppSettings:Domain"],
                        ValidAudience = configuration["AppSettings:Audience"],
#warning You cannot have secrets in your Code, for educational purposes, we will configure the secret in the appSettings.
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:SymmetricKey"])),
                    };
                });
        }
    }
}
