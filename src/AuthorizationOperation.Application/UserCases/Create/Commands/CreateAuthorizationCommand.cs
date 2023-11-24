using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.Create.Commands
{
    public class CreateAuthorizationCommand : IRequest<Authorization>
    {
        public Guid UUID { get; set; }

        public string Customer { get; set; }
    }


    public class CreateAuthorizationCommandHandler : IRequestHandler<CreateAuthorizationCommand, Authorization>
    {
        private const string messageExistsAuthorization = "Authorization {0} already exists in status {1} and was created {2}";
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<CreateAuthorizationCommandHandler> logger;
        private readonly IMediator mediator;

        public CreateAuthorizationCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAuthorizationCommandHandler> logger, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task<Authorization> Handle(CreateAuthorizationCommand cmd, CancellationToken cancellationToken)
        {
            var authorizationExists = await this.mediator.Send(new AuthorizationGetQuery() { UUID = cmd.UUID }, cancellationToken);
            if (authorizationExists != null) 
            {
                this.logger.LogInformation(messageExistsAuthorization, authorizationExists.UUID, authorizationExists.Status, authorizationExists.Created);
                throw new DomainException(string.Format(messageExistsAuthorization, authorizationExists.UUID, authorizationExists.Status, authorizationExists.Created));
            }

            var authorization = new Authorization()
            {
                UUID = cmd.UUID,
                Customer = cmd.Customer,
                StatusId = AuthorizationStatusEnum.WAITING_FOR_SIGNERS,
                Created = DateTime.UtcNow
            };

            this.unitOfWork.Repository<Authorization>().Add(authorization);
            await this.unitOfWork.Complete();

            return authorization;
        }
    }
}
