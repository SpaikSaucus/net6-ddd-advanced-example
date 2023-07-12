using AuthorizationOperation.Application.Shared.DTO;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Domain.Authorization.Queries;
using AuthorizationOperation.Domain.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.FindAll.Queries
{
    public class AuthorizationGetAllQuery : IRequest<PageDto<Authorization>>
    {
        public ushort Offset { get; set; }
        public ushort Limit { get; set; }
        public string Sort { get; set; }
        public List<AuthorizationStatusEnum> StatusIn { get; set; }
        public bool StatusInDefaultSelected { get; set; }
    }

    public class AuthorizationGetAllQueryHandler : IRequestHandler<AuthorizationGetAllQuery, PageDto<Authorization>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<AuthorizationGetAllQueryHandler> logger;

        public AuthorizationGetAllQueryHandler(IUnitOfWork unitOfWork, ILogger<AuthorizationGetAllQueryHandler> logger)
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public Task<PageDto<Authorization>> Handle(AuthorizationGetAllQuery request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("call handle AuthorizationGetAllQueryHandler.");

            if (request.StatusInDefaultSelected)
                request.StatusIn = new List<AuthorizationStatusEnum>();

            var result = new PageDto<Authorization>();
            var spec = new AuthorizationsPaginatedSpecification(request.Offset, request.Limit, request.StatusIn);

            result.Total = this.unitOfWork.Repository<Authorization>().Count(spec.Criteria);
            result.Limit = request.Limit;
            result.Offset = request.Offset;
            result.Items = this.unitOfWork.Repository<Authorization>().Find(spec);

            return Task.FromResult(result);
        }
    }
}
