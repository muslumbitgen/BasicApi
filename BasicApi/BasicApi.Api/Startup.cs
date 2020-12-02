using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using BasicApi.Common.Identity;
using BasicApi.Common.Mvc;
using BasicApi.Common.Swagger;
using BasicApi.Common.Validators;
using BasicApi.Core.Services;
using BasicApi.Items;
using BasicApi.Items.Types;
using System;
using System.Linq;
using System.Text;

namespace BasicApi.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            var origins = Configuration.GetSection("Cors").AsEnumerable().Where(x => x.Value != null).Select(x => x.Value).ToArray();

            services.AddControllersCustom(x => Extensions.CustomMvcOptions(x))
                .AddValidators(typeof(ItemsIdentifierType));

            var tokenOptions = Configuration.GetSection("TokenOptions").Get<Items.Options.TokenOptions>();

            services.AddOptions()
                   .AddContext()
                   .AddCustomIdentity()
                   .AddSwaggerDocs()
                   .AddBusinessServices()
                    .AddCors(options =>
                    {
                        options.AddDefaultPolicy(opt =>
                        {
                            opt.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
                        });
                    })
                  .AddAutoMapper(typeof(ItemsIdentifierType).Assembly)
                  .AddAuthentication(options =>
                  {
                      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

                  })
                    .AddJwtBearer(x =>
                      {
                          x.TokenValidationParameters = new TokenValidationParameters
                          {
                              ValidateIssuer = true,
                              ValidateAudience = true,
                              ValidateLifetime = true,
                              ValidIssuer = tokenOptions.Issuer,
                              ValidAudience = tokenOptions.Audience,
                              ValidateIssuerSigningKey = true,
                              IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)

                          };
                      });
            services.AddViewRenderService();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserService userService, IRoleService roleService)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDocs();

            SeedIdentity.Seed(userService, roleService, Configuration).Wait();
        }
    }
}
