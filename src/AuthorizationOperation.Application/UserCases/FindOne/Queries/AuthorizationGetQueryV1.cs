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

        public async Task<AuthorizationResponse> Handle(AuthorizationGetQueryV1 request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle AuthorizationGetQueryV1Handler.");

#warning To change when integrating the database.
            //var authorization = this.unitOfWork.Repository<Authorization>().FindById(request.Id);
            var authorization = new Authorization
            {
                Id = request.Id,
                UUID = System.Guid.NewGuid(),
                Status = new AuthorizationStatus() { Id = AuthorizationStatusEnum.WAITING_FOR_SIGNERS },
                Created = System.DateTime.UtcNow,
                Customer = "Customer 1"
            };

            return new AuthorizationResponse()
            {
                Id = authorization.Id,
                UUID = authorization.UUID,
                Customer = authorization.Customer,
                Status = authorization.Status.ToString(),
                Created = authorization.Created
            };
        }
    }
}
