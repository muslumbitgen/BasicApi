using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BasicApi.Common.Mvc;
using BasicApi.Common.Mvc.Attributes;
using BasicApi.Core.Services;
using BasicApi.Data;
using BasicApi.Items.Options;
using System;
using System.Linq;

namespace BasicApi.Api
{
    public static class Extensions
    {
        public static IServiceCollection AddContext(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();

                services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), x =>
                {
                    x.MigrationsAssembly(typeof(DataIdentifierType).Namespace);
                    x.MigrationsHistoryTable("EFMigrationsHistory", configuration.GetValue<string>("db:defaultSchemaName"));
                }));

                services.AddScoped(typeof(DbContext), typeof(ApplicationContext));
            }

            return services;
        }

        public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var provider = serviceScope.ServiceProvider;

                var db = provider.GetService<ApplicationContext>();

                var logger = provider.GetService<ILogger<Startup>>();

                db.Database.Migrate();
            }

            return app;
        }
        public static IServiceCollection AddOptions(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<PagingOptions>(configuration.GetSection("paging"));
            services.Configure<EmailOptions>(configuration.GetSection("email"));
            services.Configure<TokenOptions>(configuration.GetSection("TokenOptions"));
            services.Configure<BaseUrlOptions>(configuration.GetSection("BaseUrl"));
            services.Configure<ImageOptions>(configuration.GetSection("CloudinarySettings"));
            return services;
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IServiceBase, ServiceBase>();

            var builders = typeof(IServiceBase).Assembly
                                                 .GetTypes()
                                                 .Where(t => t.GetInterfaces()
                                                              .Any(i => i.Name == typeof(IServiceBase).Name) && t.IsClass);

            foreach (var builder in builders)
            {
                var builderInterface = builder.GetInterfaces()
                                            .Where(x => x.Name.Contains(builder.Name))
                                            .FirstOrDefault();

                if (builderInterface == null)
                {
                    throw new Exception($"Base class did not found for type: {builder.FullName}");
                }

                services.AddTransient(builderInterface, builder);
            }

            return services;
        }

        public static void CustomMvcOptions(MvcOptions options)
        {
            options.Filters.Add<ValidateModelAttribute>();

            var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
            options.Filters.Add<PagingActionFilterAttribute>();
        }


        public static IServiceCollection AddViewRenderService(this IServiceCollection services)
        {
            services.AddControllersWithViews();
            return services;
        }
    }
}
