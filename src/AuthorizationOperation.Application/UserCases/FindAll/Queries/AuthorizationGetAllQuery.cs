using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Authorization.Queries;
using AuthorizationOperation.Application.Shared.ViewModels;
using AuthorizationOperation.Application.UserCases.FindAll.ViewModels;

namespace AuthorizationOperation.Application.UserCases.FindAll.Queries
{
    public class AuthorizationGetAllQuery : IRequest<AuthorizationPageResponse>
    {
        public ushort Offset { get; set; }
        public ushort Limit { get; set; }
        public string Sort { get; set; }
        public AuthorizationCriteriaRequest Criteria { get; set; }
    }

    public class AuthorizationGetAllQueryHandler : IRequestHandler<AuthorizationGetAllQuery, AuthorizationPageResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthorizationGetAllQueryHandler> logger;

        public AuthorizationGetAllQueryHandler(IUnitOfWork unitOfWork, ILogger<AuthorizationGetAllQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public async Task<AuthorizationPageResponse> Handle(AuthorizationGetAllQuery request, CancellationToken cancellationToken)
        {
            var listStatus = new List<AuthorizationStatus>();

            if (!request.Criteria.listStatus.Contains(EnumStatusRequest.DEFAULT))
            {
                if (request.Criteria.listStatus.Any(x => x == EnumStatusRequest.CANCELLED))
                    listStatus.Add(AuthorizationStatus.CANCELLED);

                if (request.Criteria.listStatus.Any(x => x == EnumStatusRequest.WAITING_FOR_SIGNERS))
                    listStatus.Add(AuthorizationStatus.WAITING_FOR_SIGNERS);

                if (request.Criteria.listStatus.Any(x => x == EnumStatusRequest.AUTHORIZED))
                    listStatus.Add(AuthorizationStatus.AUTHORIZED);

                if (request.Criteria.listStatus.Any(x => x == EnumStatusRequest.EXPIRED))
                    listStatus.Add(AuthorizationStatus.EXPIRED);
            }

            var result = new AuthorizationPageResponse();
            var spec = new AuthorizationsPaginatedSpecification(request.Offset, request.Limit, listStatus);

            result.Total = this.unitOfWork.Repository<Authorization>().Count(spec.Criteria);
            result.Limit = request.Limit;
            result.Offset = request.Offset;

            var page = this.unitOfWork.Repository<Authorization>().Find(spec);


            result.Authorizations = page.Select(x => new AuthorizationResponse()
            {
                Id = x.Id,
                Status = x.Status.ToString(),
                Created = x.Created
            }).ToList();

            return result;
        }
    }
}
