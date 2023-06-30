using Microsoft.AspNetCore.Http;
using AuthorizationOperation.Infrastructure.Services;
using System;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Security
{
    public class HttpContextCurrentUserAccessor : ICurrentUserAccessor
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly Lazy<ClaimsBaseUser> resolver;

        public HttpContextCurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.resolver = new Lazy<ClaimsBaseUser>(() => new ClaimsBaseUser(this.httpContextAccessor.HttpContext.User.Claims), true);
        }

        public IUserInformation User => this.resolver.Value;
    }
}
