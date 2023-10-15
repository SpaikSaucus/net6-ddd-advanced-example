using AuthorizationOperation.Infrastructure.Services.ReportFile;
using Autofac;

namespace AuthorizationOperation.Infrastructure.Bootstrap.AutofacModules
{
    public class ReportFileModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReportFileExcel>()
             .As<IReportFile>()
             .InstancePerLifetimeScope()
             .Named<IReportFile>("ReportFileExcelNamed");
        }
    }
}
