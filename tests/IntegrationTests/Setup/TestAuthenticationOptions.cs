using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace IntegrationTests.Setup
{
    public sealed class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "TEST_SCHEME";

        public ClaimsIdentity Identity { get; } = new ClaimsIdentity(new[]
        {
            new Claim("Name", "test name"),
            new Claim("UserName", "test user name"),
            new Claim("Groups", ""),
            new Claim("SessionID", "test")
        }, "test");

    }
}
