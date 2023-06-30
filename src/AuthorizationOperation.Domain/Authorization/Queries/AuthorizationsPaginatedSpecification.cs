using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Domain.Authorization.Models;
using System.Collections.Generic;
using System;

namespace AuthorizationOperation.Domain.Authorization.Queries
{
    public class AuthorizationsPaginatedSpecification : BaseSpecification<Models.Authorization>
    {
        public AuthorizationsPaginatedSpecification(int skip, int take, List<AuthorizationStatus> listStatus)
        {
            if (listStatus.Count > 0)
            {
                base.AddCriteria(x => listStatus.Contains(x.Status));
            }
            else 
            {
                base.AddCriteria(x => (x.Created >= DateTime.Today 
                        && x.Status == AuthorizationStatus.AUTHORIZED 
                        || x.Status == AuthorizationStatus.CANCELLED
                    ) || x.Status == AuthorizationStatus.WAITING_FOR_SIGNERS
                );
            }

#warning Agregar el include a la lista de firmantes
            //base.AddInclude(b => b.Address);
            base.ApplyPaging(skip, take);
        }
    }
}
