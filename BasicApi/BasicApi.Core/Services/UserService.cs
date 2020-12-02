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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicApi.Items;
using BasicApi.Items.Types;
using System.Text;
using Microsoft.Extensions.Options;
using BasicApi.Items.Options;

namespace BasicApi.Core.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IViewRenderService _viewRenderService;
        private readonly IEmailService _emailService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IContextAccessor _contextAccessor;
        private IOptions<BaseUrlOptions> Options { get; }
        public UserService(ApplicationContext context,
                           IMapper mapper,
                           IOptions<BaseUrlOptions> options,
                           IContextAccessor contextAccessor,
                           UserManager<ApplicationUser> userManager,
                           IViewRenderService viewRenderService,
                           IEmailService emailService,
                           RoleManager<ApplicationRole> roleManager
                           )
        {
            _userManager = userManager;
            Options = options;
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _viewRenderService = viewRenderService;
            _roleManager = roleManager;
            _contextAccessor = contextAccessor;
        }
        public async Task<IdentityResult> CreateAsync(CreateApplicationUserCommand command)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(command);
            user.CreatedAt = DateTime.Now;
            user.CreatedBy = _contextAccessor.UserId;
            string password = PasswordExtensions.GenerateRandomPassword();

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                //new password
                var model = new TemporaryEmailViewDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = password
                };
                var html = _viewRenderService.RenderToStringAsync("EmailTemplate/_temporaryMail", model);

                await _emailService.SendEmailAsync(new SendEmailDto
                {
                    To = user.Email,
                    Message = html.Result,
                    Subject = "Yeni Parola"
                });


                //confirm email
                var token = (await _userManager.GenerateEmailConfirmationTokenAsync(user)).Base64ForUrlEncode();
                var baseUrl = Options.Value.Url;
                var confirmLink = $"{baseUrl}confirmemail?id={user.Id}&token={token}";

                var confirmModel = new ConfirmEmailViewDto
                {
                    Url = confirmLink
                };
                var confirfmHtml = _viewRenderService.RenderToStringAsync("EmailTemplate/_confirmEmail", confirmModel);

                await _emailService.SendEmailAsync(new SendEmailDto
                {
                    To = user.Email,
                    Message = confirfmHtml.Result,
                    Subject = "Hesabınızı Onaylayınız"
                });


            }
            return result;


        }
        public async Task<bool> ExistsAsync(GetApplicationUserEmailQuery query)
        {
            return await _context.Set<ApplicationUser>()
                                        .Where(x => x.Email == query.Email)
                                        .AnyAsync();
        }

        public async Task<ApplicationUserViewDto> GetAsync(GetApplicationUserQuery query)
        {
            return await _context.Set<ApplicationUser>()
                                        .Where(x => x.Id == query.Id)
                                        .Select(ApplicationUserProjectors.Project)
                                        .FirstOrDefaultAsync();
        }
        private async Task<ApplicationUser> ExistsAsync(Guid userId)
        {
            ApplicationUser user = await _context.Set<ApplicationUser>()
                                               .Where(x => x.Id == userId)
                                               .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ServiceException(ExceptionMessages.UserNotFound, errorCode: ExceptionCodes.UserNotFound);
            }

            return user;
        }

        public async Task<IdentityResult> UpdateAsync(UpdateApplicationUserCommand command)
        {
            ApplicationUser user = await ExistsAsync(command.Id);

            user.ModifiedAt = DateTime.Now;
            user.Email = command.Email;
            user.PhoneNumber = command.PhoneNumber;
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.UserName = command.UserName;
            user.About = command.About;
            user.ModifiedBy = _contextAccessor.UserId;

            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        public async Task<IdentityResult> DeleteAsync(GetApplicationUserQuery query)
        {
            ApplicationUser user = await ExistsAsync(query.Id);

            var result = await _userManager.DeleteAsync(user);

            return result;
        }

        public async Task<List<ApplicationUserViewDto>> GetAsync()
        {
            var users = new List<ApplicationUserViewDto>();

            foreach (var x in _userManager.Users.ToList())
            {
                users.Add(new ApplicationUserViewDto()
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    About = x.About,
                    LockoutEnabled = x.LockoutEnabled,
                    EmailConfirmed = x.EmailConfirmed,
                    CreatedAt = x.CreatedAt.ToString("dd.MM.yyyy"),
                    Roles = await _userManager.GetRolesAsync(x)
                });
            }
            return users.OrderByDescending(x => x.CreatedAt).ToList();
        }

        public async Task<UpdateApplicationUserActiveCommand> UpdateAsync(UpdateApplicationUserActiveCommand command)
        {
            ApplicationUser user = await ExistsAsync(command.Id);

            user.LockoutEnabled = !user.LockoutEnabled;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return command;
            }
            throw new ServiceException(result.Errors.ToList().FirstOrDefault().Description);
        }

        public async Task<List<RoleAssignViewDto>> GetAsync(GetAssignRoleQuery query)
        {
            ApplicationUser user = await ExistsAsync(query.Id);

            var roles = _roleManager.Roles.ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            List<RoleAssignViewDto> roleAssignViewDtos = new List<RoleAssignViewDto>();

            foreach (var item in roles)
            {
                RoleAssignViewDto model = new RoleAssignViewDto();
                model.RoleId = item.Id;
                model.Name = item.Name;
                model.Exists = userRoles.Contains(item.Name);
                roleAssignViewDtos.Add(model);
            }

            return roleAssignViewDtos;

        }

        public async Task CreateAsync(CreateAssignRoleCommand commands)
        {
            ApplicationUser user = await ExistsAsync(commands.UserId);

            foreach (var item in commands.UserRoles)
            {
                if (item.Exists)
                {
                    await _userManager.AddToRoleAsync(user, item.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);
                }
            }

        }

        public async Task<ApplicationUserViewDto> GetUserDetailAsync()
        {
            return await _context.Set<ApplicationUser>()
                                        .Where(x => x.Id == _contextAccessor.UserId)
                                        .Select(ApplicationUserProjectors.Project)
                                        .FirstOrDefaultAsync();
        }

        public async Task CreateToRoleAsyncAsync(CreateToRoleCommand command)
        {
            var user = await _userManager.FindByNameAsync(command.UserName);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, command.Role);
            }
        }

        public async Task<GlobalCountViewDto> GetGlobalCountAsync()
        {
            var userCount = await _context.Set<ApplicationUser>().AsQueryable().CountAsync();
            var roleCount = await _context.Set<ApplicationRole>().AsQueryable().CountAsync();
            var messageCount = await _context.Set<Feedback>().AsQueryable().CountAsync();

            GlobalCountViewDto viewDto = new GlobalCountViewDto
            {
                UserCount = userCount,
                RoleCount = roleCount,
                MessageCount = messageCount
            };
            return viewDto;
        }
    }
}
