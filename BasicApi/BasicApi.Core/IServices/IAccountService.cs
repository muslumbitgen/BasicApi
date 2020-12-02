using Microsoft.AspNetCore.Identity;
using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Queries;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public interface IAccountService
    {
        Task<string> LoginAsync(UserSignInViewDto viewDto);

        Task<string> GetForgotPasswordAsync(GetAccountForgotPasswordQuery query);

        Task<IdentityResult> ResetPasswordAsync(GetAccountResetPasswordQuery query);

        Task<IdentityResult> ConfirmEmailAsync(CreateUserConfirmCommand command);
    }
}
