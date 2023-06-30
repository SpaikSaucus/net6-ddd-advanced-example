using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ApplicationBuilder
{
    public static class SwaggerApplicationBuilderExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {            
            app.UseSwagger(c =>
            {
                //Fill the server url based on each request (host) 
                c.PreSerializeFilters.Add(ReplaceWithHostedServerUrl);
            });
            app.UseSwaggerUI(c =>
            {         
                //Build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                c.SupportedSubmitMethods(SubmitMethod.Head, SubmitMethod.Get, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete);
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(-1);
                c.EnableValidator();
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.RoutePrefix = string.Empty;
            });

        }
        
        private static void ReplaceWithHostedServerUrl(OpenApiDocument swagger, HttpRequest httpReq)
        {
            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
        }
    }
}
