using AuthorizationOperation.Domain.Core;

namespace AuthorizationOperation.Infrastructure.Services.Accessor
{
    public interface ICurrentUserAccessor
    {
        IUserInformation User { get; }
    }
}
