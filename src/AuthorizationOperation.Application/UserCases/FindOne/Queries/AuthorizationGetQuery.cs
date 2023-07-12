using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Authorization.Queries;
using AuthorizationOperation.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.FindOne.Queries
{
    public class AuthorizationGetQuery : IRequest<Authorization>
    {
        public Guid UUID { get; set; }
    }

    public class AuthorizationGetQueryHandler : IRequestHandler<AuthorizationGetQuery, Authorization>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthorizationGetQueryHandler> logger;

        public AuthorizationGetQueryHandler(IUnitOfWork unitOfWork, ILogger<AuthorizationGetQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<Authorization> Handle(AuthorizationGetQuery request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle AuthorizationGetQueryHandler.");

            var spec = new AuthorizationGetSpecification(default, request.UUID);
            return Task.FromResult(this.unitOfWork.Repository<Authorization>().Find(spec).FirstOrDefault());
        }
    }
}