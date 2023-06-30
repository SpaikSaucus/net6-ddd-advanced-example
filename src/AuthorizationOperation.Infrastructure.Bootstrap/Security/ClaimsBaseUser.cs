using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using AuthorizationOperation.Infrastructure.Services;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Security
{
    public class ClaimsBaseUser : IUserInformation
    {
        public ClaimsBaseUser(IEnumerable<Claim> claims)
        {
            var enumerable = claims as Claim[] ?? claims.ToArray();
            this.UserName = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.UseName)?.Value;
            this.Password = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.Password)?.Value;
            this.Branch = enumerable.FirstOrDefault(c => c.Type == UserClaimTypes.Branch)?.Value;
        }

        public string UserName { get; }

        public string Password { get; }

        public string Branch { get; }
    }
}
