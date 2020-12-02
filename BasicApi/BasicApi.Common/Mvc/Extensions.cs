using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using BasicApi.Items.Types;
using System;

namespace BasicApi.Common.Mvc
{
    public static class Extensions
    {
        public static IMvcBuilder AddControllersCustom(this IServiceCollection services, Action<MvcOptions> mvcOptions = null)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IContextAccessor, ContextAccessor>();

            return services.AddControllers(mvcOptions)
                           .AddNewtonsoftJson(o =>
                           {
                               o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                               o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                               o.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                               o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                               o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                               o.SerializerSettings.Formatting = Formatting.Indented;
                               o.SerializerSettings.Converters.Add(new StringEnumConverter());
                           })
                           .AddJsonOptions(o =>
                           {
                           });
        }

        public static void CustomMvcOptions(MvcOptions x)
        {
            throw new NotImplementedException();
        }

        public static bool IsDocker(this IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName == "Docker")
            {
                return true;
            }

            return false;
        }

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
