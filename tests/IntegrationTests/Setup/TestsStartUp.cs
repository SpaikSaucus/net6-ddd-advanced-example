using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthorizationOperation.Infrastructure.Bootstrap;

namespace AuthorizationOperation.IntegrationTests.Setup
{
    public class TestsStartUp : ApplicationStartup
    {
        public TestsStartUp(IConfiguration configuration)
            : base(configuration)
        {

        }

        public override void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser().AddAuthenticationSchemes(TestAuthenticationOptions.Scheme);
                });
            });

            services.AddAuthentication().AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TestAuthenticationOptions.Scheme, null);
        }
    }
}
