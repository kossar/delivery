using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApp
{
    /// <summary>
    /// Swagger configuration options
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Swagger configuration options constructor
        /// </summary>
        /// <param name="provider">IApiVersionDescriptionProvider</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Swagger configuration
        /// </summary>
        /// <param name="options">SwaggerGenOptions</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var apiVersionDescription in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    apiVersionDescription.GroupName,
                    new OpenApiInfo()
                    {
                        Title = $"API {apiVersionDescription.ApiVersion}",
                        Version = apiVersionDescription.ApiVersion.ToString(),
                    }
                );
            }
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            
            // Use fullname for schema id
            options.CustomSchemaIds(i => i.FullName);
            
            // Add support for authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme.\r\n<br>" +
                    "Enter 'Bearer'[space] and then your token in the text box below.\r\n<br>" +
                    "Example: <b>Bearer eyJhbGciOiJIUzUxMiIsIn...</b>\r\n<br>" +
                    "You will get the bearer from the <i>account/login</i> or <i>account/register</i> endpoint.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        }
    }
}