using AuthorizationOperation.Application.Shared.ViewModels;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.FindOne.Queries
{
    public class AuthorizationGetQueryV1 : IRequest<AuthorizationResponse>
    {
        public uint Id { get; set; }
    }

    public class AuthorizationGetQueryV1Handler : IRequestHandler<AuthorizationGetQueryV1, AuthorizationResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthorizationGetQueryV1Handler> logger;

        public AuthorizationGetQueryV1Handler(IUnitOfWork unitOfWork, ILogger<AuthorizationGetQueryV1Handler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<AuthorizationResponse> Handle(AuthorizationGetQueryV1 request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle AuthorizationGetQueryV1Handler.");

            var authorization = this.unitOfWork.Repository<Authorization>().FindById(request.Id);
            if (authorization != null)
            {
                return Task.FromResult(new AuthorizationResponse()
                {
                    Id = authorization.Id,
                    UUID = authorization.UUID,
                    Customer = authorization.Customer,
                    Status = authorization.Status.Name.ToString(),
                    Created = authorization.Created
                });
            }
            else
            {
                return Task.FromResult<AuthorizationResponse>(null);
            }
        }
    }
}
