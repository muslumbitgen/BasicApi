using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BasicApi.Core.Services;
using BasicApi.Items.Commands;
using BasicApi.Items.Queries;

namespace BasicApi.Api.Controllers
{
    [Route("api/basicapi/v1/role"), AllowAnonymous]
    [Authorize(Roles = "Admin")]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Create New Role
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateApplicationRoleCommand command)
        {
            return Ok(await _roleService.CreateAsync(command));
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut, Route("{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateApplicationRoleCommand command)
        {
            command.Id = id;
            return Ok(await _roleService.UpdateAsync(command));
        }

        /// <summary>
        /// Get Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("{id:guid}")]
        public async Task<ActionResult> GetAsync([FromRoute] Guid id)
        {
            GetApplicationRoleQuery query = new GetApplicationRoleQuery()
            {
                Id = id
            };

            return Ok(await _roleService.GetAsync(query));
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete, Route("{id:guid}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            GetApplicationRoleQuery query = new GetApplicationRoleQuery()
            {
                Id = id
            };

            return Ok(await _roleService.DeleteAsync(query));
        }
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _roleService.GetAsync());
        }

    }
}