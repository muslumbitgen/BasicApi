using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BasicApi.Core.Services;
using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Queries;

namespace BasicApi.Api.Controllers
{
    [Route("api/BasicApi/v1/account"), AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        /// <summary>
        /// User login
        /// </summary>
        /// <param name="viewDto"></param>
        /// <returns></returns>
        [HttpPost, Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserSignInViewDto viewDto)
        {
            var token = await _accountService.LoginAsync(viewDto);
            if (token != null)
            {
                return Ok(new { token });
            }
            return BadRequest();
        }

        /// <summary>
        /// User ForgotPassword Token
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet, Route("forgotpassword/{email}")]
        public async Task<IActionResult> GetForgotPasswordAsync([FromRoute] string email)
        {
            GetAccountForgotPasswordQuery query = new GetAccountForgotPasswordQuery
            {
                Email = email
            };
            var token = await _accountService.GetForgotPasswordAsync(query);
            return Ok(new { token });
        }
        /// <summary>
        /// User ResetPassword
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost, Route("resetpassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] GetAccountResetPasswordQuery query)
        {
            return Ok(await _accountService.ResetPasswordAsync(query));
        }


        /// <summary>
        /// User ConfirmEmail
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost, Route("confirmemail")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserConfirmCommand command)
        {
            return Ok(await _accountService.ConfirmEmailAsync(command));
        }

    }
}