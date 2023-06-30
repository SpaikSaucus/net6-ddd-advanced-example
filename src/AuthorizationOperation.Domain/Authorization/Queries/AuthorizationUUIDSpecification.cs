using AuthorizationOperation.Domain.Core;
using System;

namespace AuthorizationOperation.Domain.Authorization.Queries
{
    public class AuthorizationUUIDSpecification : BaseSpecification<Models.Authorization>
    {
        public AuthorizationUUIDSpecification(Guid uuid)
        {
            base.AddCriteria(x => x.UUID == uuid);
        }
    }
}
