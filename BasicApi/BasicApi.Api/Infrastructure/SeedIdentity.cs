using Microsoft.Extensions.Configuration;
using BasicApi.Core.Services;
using BasicApi.Items.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicApi.Api
{
    public class SeedIdentity
    {
        public static async Task Seed(IUserService userService, IRoleService roleService, IConfiguration configuration)
        {
            var username = configuration["Data:UserName"];
            var email = configuration["Data:Email"];
            var firstName = configuration["Data:FirstName"];
            var lastName = configuration["Data:LastName"];
            var phoneNumber = configuration["Data:PhoneNumber"];
            var about = configuration["Data:About"];
            var role = configuration["Data:Role"];

            CreateApplicationUserCommand user = new CreateApplicationUserCommand
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                Email = email,
                PhoneNumber = phoneNumber,
                About = about
            };
            CreateApplicationRoleCommand roleCommand = new CreateApplicationRoleCommand
            {
                Name = role
            };
            var userCount = await userService.GetAsync();
            if (userCount.Count() == 0)
            {
                await roleService.CreateAsync(roleCommand);
                var result = await userService.CreateAsync(user);

                if (result.Succeeded)
                {
                    CreateToRoleCommand command = new CreateToRoleCommand
                    {
                        UserName = user.UserName,
                        Role = roleCommand.Name
                    };
                    await userService.CreateToRoleAsyncAsync(command);
                }
            }
        }

    }
}
