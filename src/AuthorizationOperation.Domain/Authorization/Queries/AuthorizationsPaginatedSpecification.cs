using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Domain.Authorization.Models;
using System.Collections.Generic;
using System;

namespace AuthorizationOperation.Domain.Authorization.Queries
{
    public class AuthorizationsPaginatedSpecification : BaseSpecification<Models.Authorization>
    {
        public AuthorizationsPaginatedSpecification(int skip, int take, List<AuthorizationStatusEnum> listStatus)
        {
            base.AddInclude(x => x.Status);

            if (listStatus.Count > 0)
            {
                base.AddCriteria(x => listStatus.Contains(x.Status.Id));
            }
            else 
            {
                base.AddCriteria(x => x.Status.Id == AuthorizationStatusEnum.WAITING_FOR_SIGNERS
                        || (x.Created >= DateTime.Today && x.Status.Id == AuthorizationStatusEnum.AUTHORIZED || x.Status.Id == AuthorizationStatusEnum.CANCELLED)
                );
            }

            base.ApplyPaging(skip, take);
        }
    }
}
