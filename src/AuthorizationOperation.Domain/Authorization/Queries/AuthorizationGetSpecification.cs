using AuthorizationOperation.Domain.Core;
using System;
using System.Linq.Expressions;

namespace AuthorizationOperation.Domain.Authorization.Queries
{
    public class AuthorizationGetSpecification : BaseSpecification<Models.Authorization>
    {
        public AuthorizationGetSpecification(uint id = default, Guid uuid = default)
        {
            base.AddInclude(x => x.Status);

            Expression<Func<Models.Authorization, bool>> criteria = null;


            if (id == default && uuid == default)
                throw new ArgumentNullException(nameof(id));

            if (id != default)
            {
                criteria = this.OrCriteria(criteria, x => x.Id == id);
            }

            if (uuid != default)
            {
                criteria = this.OrCriteria(criteria, x => x.UUID == uuid);
            }

            base.SetCriteria(criteria);
        }
    }
}
