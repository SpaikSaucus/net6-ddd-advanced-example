﻿using AuthorizationOperation.Application.UserCases.Create.ViewModels;
using AuthorizationOperation.Application.UserCases.FindOne.Queries;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.Create.Command
{
    public class CreateAuthorizationCommand : IRequest<CreateAuthorizationResponse>
    {
        public CreateAuthorizationRequest Request { get; set; }
    }


    public class CreateAuthorizationCommandHandler : IRequestHandler<CreateAuthorizationCommand, CreateAuthorizationResponse>
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

        public async Task<CreateAuthorizationResponse> Handle(CreateAuthorizationCommand cmd, CancellationToken cancellationToken)
        {
            var authorizationExists = await this.mediator.Send(new AuthorizationGetQuery() { UUID = cmd.Request.UUID }, cancellationToken);
            if (authorizationExists != null) 
            {
                this.logger.LogInformation(messageExistsAuthorization, authorizationExists.UUID, authorizationExists.Status, authorizationExists.Created);
                return new CreateAuthorizationResponse() { Id = authorizationExists.Id, Status = authorizationExists.Status };
            }

            var authorization = new Authorization()
            {
                UUID = cmd.Request.UUID,
                Customer = cmd.Request.Customer,
                Created = DateTime.UtcNow
            };

            this.unitOfWork.Repository<Authorization>().Add(authorization);
            await this.unitOfWork.Complete();

            return new CreateAuthorizationResponse() { Id = authorization.Id, Status = AuthorizationStatusEnum.WAITING_FOR_SIGNERS.ToString() };
        }
    }
}