using AuthorizationOperation.Application.UserCases.FindAll.Queries;
using AuthorizationOperation.Domain.Authorization.Models;
using AuthorizationOperation.Infrastructure.Services.ReportFile;
using Autofac;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AuthorizationOperation.Application.UserCases.Report.Commands
{
    public class ReportFileExcelCommand : IRequest<MemoryStream>
    {
        public List<AuthorizationStatusEnum> StatusIn { get; set; }
    }

    public class ReportFileExcelHandler : IRequestHandler<ReportFileExcelCommand, MemoryStream>
    {
        private readonly ILogger<ReportFileExcelHandler> logger;
        private readonly IMediator mediator;
        private readonly IComponentContext context;

        public ReportFileExcelHandler(
            ILogger<ReportFileExcelHandler> logger,
            IMediator mediator,
            IComponentContext context)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.context = context;
        }

        public async Task<MemoryStream> Handle(ReportFileExcelCommand cmd, CancellationToken cancellationToken)
        {
            this.logger.LogDebug("Get Authorizations");
            var query = new AuthorizationGetAllQuery() { 
                StatusIn = cmd.StatusIn,
                Limit = 10000
            };
            var dto = await this.mediator.Send(query, cancellationToken);

            this.logger.LogDebug("Build XLSX");

            IReportFile service = this.context.ResolveNamed<IReportFile>("ReportFileExcelNamed");
            if (service == null)
                throw new Exception("Error service GenerateFileExcel not exists.");

            return service.Create(dto.Items);
        }
    }
}
