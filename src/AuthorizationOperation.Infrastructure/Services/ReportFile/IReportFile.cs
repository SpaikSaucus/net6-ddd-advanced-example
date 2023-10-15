using AuthorizationOperation.Domain.Authorization.Models;
using System.Collections.Generic;
using System.IO;

namespace AuthorizationOperation.Infrastructure.Services.ReportFile
{
    public interface IReportFile
    {
        MemoryStream Create(IEnumerable<Authorization> authorizationOperations);
    }
}
