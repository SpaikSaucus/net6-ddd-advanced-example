using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using AuthorizationOperation.Infrastructure.Services;
using AuthorizationOperation.Domain.Core;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Security
{
    public class ClaimsBaseUser : IUserInformation
    {
        public ClaimsBaseUser(IEnumerable<Claim> claims)
        {
            var enumerable = claims as Claim[] ?? claims.ToArray();
            this.Guid = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.Guid)?.Value;
            this.UserName = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.UserName)?.Value;
            this.FirstName = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.FirstName)?.Value;
            this.LastName = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.LastName)?.Value;
            this.Email = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.Email)?.Value;
        }

        public string Guid { get; }
        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}
