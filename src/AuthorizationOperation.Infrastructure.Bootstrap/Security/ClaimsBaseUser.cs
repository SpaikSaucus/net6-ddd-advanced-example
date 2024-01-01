using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Security
{
    public class ClaimsBaseUser : IUserInformation
    {
        private const string ErrorClaimNotExist = "The UserClaims is not complete";

        public ClaimsBaseUser(IEnumerable<Claim> claims)
        {
            var enumerable = claims as Claim[] ?? claims.ToArray();
            try
            {
                this.Guid = enumerable.First(c => c.Type == UserClaimTypes.Guid).Value;
                this.UserName = enumerable.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                this.FirstName = enumerable.First(c => c.Type == ClaimTypes.Name).Value;
                this.LastName = enumerable.First(c => c.Type == ClaimTypes.Surname).Value;
                this.Email = enumerable.First(c => c.Type == ClaimTypes.Email).Value;
            }
            catch (Exception)
            {
                throw new TechnicalException(ErrorClaimNotExist);
            }
        }

        public string Guid { get; }
        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}
