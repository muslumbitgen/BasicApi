using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BasicApi.Data;
using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Entities;
using BasicApi.Items.Exceptions;
using BasicApi.Items.Projectors;
using BasicApi.Items.Queries;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public class RoleService : ServiceBase, IRoleService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IContextAccessor _contextAccessor;
        private readonly IEmailService _emailService;
        private readonly IViewRenderService _viewRenderService;

        public RoleService(ApplicationContext context,
                            IMapper mapper,
                            RoleManager<ApplicationRole> roleManager,
                            IContextAccessor contextAccessor,
                            IViewRenderService viewRenderService,
                            IEmailService emailService)
        {
            _viewRenderService = viewRenderService;
            _emailService = emailService;
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
            _contextAccessor = contextAccessor;

        }

        public async Task<IdentityResult> CreateAsync(CreateApplicationRoleCommand command)
        {
            ApplicationRole role = _mapper.Map<ApplicationRole>(command);
            role.CreatedAt = DateTime.Now;
            role.CreatedBy = _contextAccessor.UserId;
            var result = await _roleManager.CreateAsync(role);
            return result;
        }

        public async Task<IdentityResult> DeleteAsync(GetApplicationRoleQuery query)
        {
            var role = _roleManager.Roles.FirstOrDefault(I => I.Id == query.Id);

            var result = await _roleManager.DeleteAsync(role);

            return result;

        }

        public async Task<bool> ExistsAsync(GetApplicationRoleQuery query)
        {
            return await _context.Set<ApplicationRole>()
                                               .Where(x => x.Id == query.Id)
                                               .AnyAsync();
        }

        public async Task<ApplicationRoleViewDto> GetAsync(GetApplicationRoleQuery query)
        {
            return await _context.Set<ApplicationRole>()
                                                  .Where(x => x.Id == query.Id)
                                                  .OrderByDescending(x => x.CreatedAt)
                                                  .Select(ApplicationRoleProjectors.Project)
                                                  .FirstOrDefaultAsync();
        }

        public async Task<List<ApplicationRoleViewDto>> GetAsync()
        {
            return await _context.Set<ApplicationRole>()
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Select(ApplicationRoleProjectors.Project)
                                    .ToListAsync();
        }

        public async Task<IdentityResult> UpdateAsync(UpdateApplicationRoleCommand command)
        {
            ApplicationRole role = await ExistsAsync(command.Id);

            role.ModifiedAt = DateTime.Now;
            role.Name = command.Name;
            role.ModifiedBy = _contextAccessor.UserId;

            var result = await _roleManager.UpdateAsync(role);

            return result;
        }

        private async Task<ApplicationRole> ExistsAsync(Guid roleId)
        {
            ApplicationRole role = await _context.Set<ApplicationRole>()
                                               .Where(x => x.Id == roleId)
                                               .FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ServiceException(ExceptionMessages.RoleNotFound, errorCode: ExceptionCodes.RoleNotFound);
            }

            return role;
        }
    }
}
