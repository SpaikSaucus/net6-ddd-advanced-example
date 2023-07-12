using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace AuthorizationOperation.Infrastructure.Bootstrap.Extensions.ServiceCollection
{
    public static class SwaggerServiceCollectionExtensions
    {
        private const string BearerSecurityScheme = "Bearer";
        
        public static void AddSwaggerGenExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                    c.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));

                //Support Bearer tokens sent in the 'Authorization' header.
                c.AddSecurityDefinition(BearerSecurityScheme, new OpenApiSecurityScheme
                {
                    Scheme = BearerSecurityScheme,
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter your JWT token in the text input below. You can omit the 'Bearer ' prefix ;)",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Name = HeaderNames.Authorization
                });
                
                //Apply Bearer scheme to all scopes/endpoints. See https://swagger.io/docs/specification/authentication/bearer-authentication/
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = BearerSecurityScheme,
                                Type = ReferenceType.SecurityScheme
                            },
                            Name = BearerSecurityScheme,
                            In = ParameterLocation.Header,
                            BearerFormat = "JWT"
                        }, 
                        new List<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{GetApplicationName()}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = GetApplicationName(),
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
                info.Description += "Deprecated API.";
            
            return info;
        }

        private static string GetApplicationName()
        {
            return (System.Reflection.Assembly.GetEntryAssembly() ?? System.Reflection.Assembly.GetExecutingAssembly()).GetName().Name;
        }
    }
}
