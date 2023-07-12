using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Authorization.Queries;
using AuthorizationOperation.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.FindOne.Queries
{
    public class AuthorizationGetQueryV1 : IRequest<Authorization>
    {
        public uint Id { get; set; }
    }

    public class AuthorizationGetQueryV1Handler : IRequestHandler<AuthorizationGetQueryV1, Authorization>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthorizationGetQueryV1Handler> logger;

        public AuthorizationGetQueryV1Handler(IUnitOfWork unitOfWork, ILogger<AuthorizationGetQueryV1Handler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<Authorization> Handle(AuthorizationGetQueryV1 request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle AuthorizationGetQueryV1Handler.");

            var spec = new AuthorizationGetSpecification(request.Id);
            return Task.FromResult(this.unitOfWork.Repository<Authorization>().Find(spec).FirstOrDefault());
        }
    }
}
