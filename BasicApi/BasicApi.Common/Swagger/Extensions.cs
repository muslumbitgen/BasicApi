using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using BasicApi.Items.Options;
using System;
using System.IO;
using System.Text;

namespace BasicApi.Common.Swagger
{
    public static class Extensions
    {
        public static IServiceCollection AddSwaggerDocs(this IServiceCollection services)
        {
            SwaggerOptions options;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                services.Configure<SwaggerOptions>(configuration.GetSection("swagger"));

                options = configuration.GetOptions<SwaggerOptions>("swagger");
            }

            if (!options.Enabled)
            {
                return services;
            }

            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(options.Name, new OpenApiInfo { Title = options.Title, Version = options.Version });

                if (options.IncludeSecurity)
                {
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Name = "Bearer",
                                Scheme = "oauth2",
                                In = ParameterLocation.Header
                            },
                            new string[] { }
                        }
                    });
                }

                if (options.CommentsEnabled)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, options.XmlFileName);

                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        public static IApplicationBuilder UseSwaggerDocs(this IApplicationBuilder builder)
        {
            var options = builder.ApplicationServices
                                 .GetService<IConfiguration>()
                                 .GetOptions<SwaggerOptions>("swagger");

            if (!options.Enabled)
            {
                return builder;
            }

            var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "swagger" : options.RoutePrefix;

            builder.UseStaticFiles()
                .UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
                c.RoutePrefix = routePrefix;
            });

            if (options.ReDocEnabled)
            {
                builder.UseReDoc(c =>
                {
                    c.RoutePrefix = $"{routePrefix}/redoc";
                    c.SpecUrl = $"/{routePrefix}/{options.Name}/swagger.json";
                });
            }

            return builder;
        }
    }
}
