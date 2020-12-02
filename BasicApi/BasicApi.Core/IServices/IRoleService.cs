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
    public interface IRoleService
    {
        Task<IdentityResult> CreateAsync(CreateApplicationRoleCommand command);

        Task<IdentityResult> UpdateAsync(UpdateApplicationRoleCommand command);

        Task<bool> ExistsAsync(GetApplicationRoleQuery query);

        Task<ApplicationRoleViewDto> GetAsync(GetApplicationRoleQuery query);

        Task<List<ApplicationRoleViewDto>> GetAsync();

        Task<IdentityResult> DeleteAsync(GetApplicationRoleQuery query);
    }
}
