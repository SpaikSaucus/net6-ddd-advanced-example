using AuthorizationOperation.Application.Shared.ViewModels;
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
    public class AuthorizationGetQuery : IRequest<AuthorizationResponse>
    {
        public Guid UUID { get; set; }
    }

    public class AuthorizationGetQueryHandler : IRequestHandler<AuthorizationGetQuery, AuthorizationResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthorizationGetQueryHandler> logger;

        public AuthorizationGetQueryHandler(IUnitOfWork unitOfWork, ILogger<AuthorizationGetQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<AuthorizationResponse> Handle(AuthorizationGetQuery request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle AuthorizationGetQueryHandler.");

            var spec = new AuthorizationUUIDSpecification(request.UUID);
            var authorization = this.unitOfWork.Repository<Authorization>().Find(spec).FirstOrDefault();

            return new AuthorizationResponse()
            {
                Id = authorization.Id,
                Customer = authorization.Customer,
                Status = authorization.Status.ToString(),
                Created = authorization.Created
            };
        }
    }
}