using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Domain.User.Models;
using AuthorizationOperation.Domain.User.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.FindOne.Queries
{
    public class UserGetQuery : IRequest<User>
    {
        public string UserName{ get; set; }
        public string Password { get; set; }
    }

    public class UserGetQueryHandler : IRequestHandler<UserGetQuery, User>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<UserGetQueryHandler> logger;

        public UserGetQueryHandler(IUnitOfWork unitOfWork, ILogger<UserGetQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<User> Handle(UserGetQuery request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle UserGetQueryHandler.");

            var spec = new UserGetSpecification(request.UserName, request.Password);
            return Task.FromResult(this.unitOfWork.Repository<User>().Find(spec).FirstOrDefault());
        }
    }
}
