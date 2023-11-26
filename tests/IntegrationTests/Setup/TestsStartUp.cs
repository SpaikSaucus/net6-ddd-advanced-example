using AuthorizationOperation.Application.Behaviors;
using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Infrastructure.Bootstrap.AutofacModules;
using AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ApplicationBuilder;
using AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ServiceCollection;
using AuthorizationOperation.Infrastructure.Bootstrap.Security;
using AuthorizationOperation.Infrastructure.Core;
using AuthorizationOperation.Infrastructure.Services.Accessor;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace IntegrationTests.Setup
{
	public class TestsStartup
    {
		protected const string JwtPolicy = "JwtPolicy";
		private readonly IConfiguration configuration;
		private readonly IWebHostEnvironment env;

		public TestsStartup(IConfiguration configuration, IWebHostEnvironment env)
		{
			this.configuration = configuration;
			this.env = env;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(TestAuthenticationOptions.Scheme).AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TestAuthenticationOptions.Scheme, null);

			services.AddCorsExtension();
			services.AddApiVersionExtension("2.0");
			services.AddHealthChecksExtension();
			services.AddSwaggerGenExtension();
			services.AddResponseCompressionExtension();
			services.AddHttpClientFactoryExtension(this.configuration);
			services.AddHttpContextAccessor();

			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ValidatorBehavior<,>).Assembly));
			services.AddControllers(o =>
			{
				o.Filters.Add(new ProducesResponseTypeAttribute(400));
				o.Filters.Add(new ProducesResponseTypeAttribute(500));
			}).AddJsonOptions(o =>
			{
				o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			});

			services.AddDbContext<AuthorizationMockDbContext>(opt =>
			{
				opt.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			});

			services.AddTransient<IUnitOfWork, UnitOfWork>();
		}

		public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
		{
			app.UseCorsExtension();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseHealthChecksExtension();
			app.UseResponseCompression();
			app.UseSwaggerExtension(provider);

			app.UseExceptionHandler(errorPipeline =>
			{
				errorPipeline.UseExceptionHandlerMiddleware(true);
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
				endpoints.MapControllers();
			});
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			_ = builder.RegisterType<HttpContextCurrentUserAccessor>()
				.As<ICurrentUserAccessor>()
				.InstancePerLifetimeScope();

			builder.RegisterModule(new InfrastructureMockModule());
			builder.RegisterModule(new MediatorModule(true));
			builder.RegisterModule(new ReportFileModule());
		}
	}
}
