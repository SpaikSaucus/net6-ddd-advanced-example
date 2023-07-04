using AuthorizationOperation.Domain.Core;
using System;

namespace AuthorizationOperation.Domain.Authorization.Queries
{
    public class AuthorizationUUIDSpecification : BaseSpecification<Models.Authorization>
    {
        public AuthorizationUUIDSpecification(Guid uuid)
        {
            base.AddInclude(x => x.Status);
            base.AddCriteria(x => x.UUID == uuid);
        }
    }
}
