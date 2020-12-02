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
    [Route("api/basicapi/v1/user")]
    [Authorize(Roles = "Admin")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Create New User
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateApplicationUserCommand command)
        {
            return Ok(await _userService.CreateAsync(command));
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut, Route("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateApplicationUserCommand command)
        {
            command.Id = id;
            return Ok(await _userService.UpdateAsync(command));
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult> GetAsync([FromRoute] Guid id)
        {
            GetApplicationUserQuery query = new GetApplicationUserQuery()
            {
                Id = id
            };

            return Ok(await _userService.GetAsync(query));
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id:guid}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            GetApplicationUserQuery query = new GetApplicationUserQuery()
            {
                Id = id
            };

            return Ok(await _userService.DeleteAsync(query));
        }
        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _userService.GetAsync());
        }
        /// <summary>
        /// Update User Active
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut, Route("{id:guid}/userstate")]
        public async Task<IActionResult> UserStateAsync([FromRoute] Guid id)
        {
            UpdateApplicationUserActiveCommand command = new UpdateApplicationUserActiveCommand
            {
                Id = id
            };
            return Ok(await _userService.UpdateAsync(command));
        }
        /// <summary>
        /// Get Assign Role
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("assing/{id:guid}")]
        public async Task<IActionResult> GetAssignAsync([FromRoute] Guid id)
        {
            GetAssignRoleQuery query = new GetAssignRoleQuery
            {
                Id = id
            };
            return Ok(await _userService.GetAsync(query));
        }
        /// <summary>
        /// create Assign Role
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("assing")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAssignRoleCommand command)
        {
            await _userService.CreateAsync(command);
            return Ok();
        }
        /// <summary>
        /// Get User Detail
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("userdetail")]
        public async Task<IActionResult> GetUserDetailAsync()
        {
            return Ok(await _userService.GetUserDetailAsync());
        }

        /// <summary>
        /// Get Global Count
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("globalcount")]
        public async Task<IActionResult> GetGlobalCountAsync()
        {
            return Ok(await _userService.GetGlobalCountAsync());
        }
    }
}