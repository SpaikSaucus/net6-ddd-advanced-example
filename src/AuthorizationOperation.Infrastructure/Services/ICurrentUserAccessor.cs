using AuthorizationOperation.Domain.Core;

namespace AuthorizationOperation.Infrastructure.Services
{
    public interface ICurrentUserAccessor
    {
        IUserInformation User { get; }
    }
}
