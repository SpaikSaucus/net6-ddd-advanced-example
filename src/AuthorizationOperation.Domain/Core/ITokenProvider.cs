using System.Collections.Generic;
using System.Security.Claims;

namespace AuthorizationOperation.Domain.Core
{
    public interface ITokenProvider
    {
        string GenerateToken(IUserInformation userInformation);
    }
}
