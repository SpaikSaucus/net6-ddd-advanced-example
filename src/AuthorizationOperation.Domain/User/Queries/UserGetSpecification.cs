using AuthorizationOperation.Domain.Core;
using System;
using System.Linq.Expressions;

namespace AuthorizationOperation.Domain.User.Queries
{
    public class UserGetSpecification : BaseSpecification<Models.User>
    {
        public UserGetSpecification(string userName, string password = default)
        {
            Expression<Func<Models.User, bool>> criteria = x => x.UserName == userName;

            if (password != default)
            {
                criteria = this.AndCriteria(criteria, x => x.Password == password);
            }

            base.SetCriteria(criteria);
        }
    }
}
