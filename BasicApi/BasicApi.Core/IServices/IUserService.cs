using Microsoft.AspNetCore.Identity;
using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(CreateApplicationUserCommand command);

        Task<IdentityResult> UpdateAsync(UpdateApplicationUserCommand command);

        Task<UpdateApplicationUserActiveCommand> UpdateAsync(UpdateApplicationUserActiveCommand command);

        Task<bool> ExistsAsync(GetApplicationUserEmailQuery query);

        Task<ApplicationUserViewDto> GetAsync(GetApplicationUserQuery query);

        Task<ApplicationUserViewDto> GetUserDetailAsync();

        Task<List<RoleAssignViewDto>> GetAsync(GetAssignRoleQuery query);

        Task<List<ApplicationUserViewDto>> GetAsync();

        Task<IdentityResult> DeleteAsync(GetApplicationUserQuery query);

        Task CreateAsync(CreateAssignRoleCommand commands);

        Task CreateToRoleAsyncAsync(CreateToRoleCommand command);

        Task<GlobalCountViewDto> GetGlobalCountAsync();
    }
}
