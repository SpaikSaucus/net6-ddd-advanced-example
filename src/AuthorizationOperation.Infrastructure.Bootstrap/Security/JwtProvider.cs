using AuthorizationOperation.Domain.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Security
{
    public class JwtProvider : ITokenProvider
    {
        private readonly IConfiguration Configuration ;
        public JwtProvider(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public string GenerateToken(IUserInformation userInformation) 
        {
            var claims = new List<Claim>
            {
                new Claim(UserClaimTypes.Guid, userInformation.Guid),
                new Claim(UserClaimTypes.UserName, userInformation.UserName),
                new Claim(UserClaimTypes.FirstName, userInformation.FirstName),
                new Claim(UserClaimTypes.LastName, userInformation.LastName),
                new Claim(UserClaimTypes.Email, userInformation.Email)
            };

            var secretKey = this.Configuration["AppSettings:SymmetricKey"]; //TODO: you can't have secrets in your Code (in this case, in the AppSettings)
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = claims.ToDictionary(x => x.Type, x => (object)x.Value),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }
    }
}
