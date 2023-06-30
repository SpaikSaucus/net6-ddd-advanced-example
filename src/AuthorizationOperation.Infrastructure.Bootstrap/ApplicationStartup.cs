using Autofac;
using AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ApplicationBuilder;
using AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ServiceCollection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthorizationOperation.Domain.Core;
using AuthorizationOperation.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AuthorizationOperation.Infrastructure.Bootstrap
{
    public class ApplicationStartup
    {
        private readonly IConfiguration configuration;
        protected const string JwtPolicy = "JwtPolicy";        

        public ApplicationStartup(IConfiguration configuration)
        {
            this.configuration = configuration;            
        }

        public virtual void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtPolicy, policy =>
                {
                    policy.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                });
            });
            services.AddAuthenticationExtension(this.configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.AddAutofacExtension(this.configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsExtension();

            services.AddApiVersionExtension(this.configuration.GetValue<string>("AppSettings:DefaultApiVersion", "1.0"));
            services.AddHealthChecksExtension();
            services.AddSwaggerGenExtension();
            services.AddResponseCompressionExtension();
            services.AddHttpClientFactoryExtension(this.configuration);
            services.AddHttpContextAccessor();
           
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Behaviors.ValidatorBehavior<,>).Assembly));
            services.AddControllers(o =>
            {
                o.Filters.Add(new ProducesResponseTypeAttribute(400));
                o.Filters.Add(new ProducesResponseTypeAttribute(500));
            });

            services.AddDbContext<AuthorizationDbContext>(opt =>
            {
                opt.UseMySQL(this.configuration.GetValue<string>("ConnectionStrings"));
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                //Do Something...
            }
            app.UseCorsExtension();

            app.UseRouting();
            app.UseAuthentication();
            app.UseHealthChecksExtension();
            app.UseResponseCompression();
            app.UseSwaggerExtension(provider);
       
            app.UseExceptionHandler(errorPipeline =>
            {
                errorPipeline.UseExceptionHandlerMiddleware(this.configuration.GetValue("AppSettings:IncludeErrorDetailInResponse", false));
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }
    }
}
