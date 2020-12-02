using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using BasicApi.Data;
using BasicApi.Items.Entities;
using BasicApi.Items.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Common.Identity
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                    .AddDefaultTokenProviders();


            services.Configure<IdentityOptions>(options =>
            {
                // password

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            return services;
        }
    }
}
