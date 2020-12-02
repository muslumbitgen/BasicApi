using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BasicApi.Data;
using BasicApi.Items;
using BasicApi.Items.Commands;
using BasicApi.Items.Dtos;
using BasicApi.Items.Entities;
using BasicApi.Items.Exceptions;
using BasicApi.Items.Options;
using BasicApi.Items.Projectors;
using BasicApi.Items.Queries;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public class AccountService : ServiceBase, IAccountService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IViewRenderService _viewRenderService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IOptions<BaseUrlOptions> Options { get; }

        public AccountService(ApplicationContext context,
                         IMapper mapper,
                         IOptions<BaseUrlOptions> options,
                         UserManager<ApplicationUser> userManager,
                         IViewRenderService viewRenderService,
                         IEmailService emailService,
                         ITokenService tokenService,
                         SignInManager<ApplicationUser> signInManager
                         )
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _context = context;
            Options = options;
            _mapper = mapper;
            _emailService = emailService;
            _viewRenderService = viewRenderService;
            _signInManager = signInManager;

        }

        public async Task<IdentityResult> ConfirmEmailAsync(CreateUserConfirmCommand command)
        {
            if (command.Id == null || command.Token == null)
            {
                throw new ServiceException("Hesap onayı için bilgileriniz yanlıştır.");
            }
            var user = await _userManager.FindByIdAsync(command.Id.ToString());
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, command.Token.Base64ForUrlDecode());
                return result;

            }
            throw new ServiceException("Hesabınız onaylanamadı.");
        }

        public async Task<string> GetForgotPasswordAsync(GetAccountForgotPasswordQuery query)
        {
            var user = await _userManager.FindByEmailAsync(query.Email);
            if (user == null)
            {
                throw new ServiceException(query.Email + "Eposta adresi ile bir kullanıcı bulunamadı.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ServiceException("Lütfen hesabınızı email ile onaylayınız.");
            }
            var baseUrl = Options.Value.Url;
            var token = (await _userManager.GeneratePasswordResetTokenAsync(user)).Base64ForUrlEncode();
            var confirmLink = $"{baseUrl}resetpassword?id={user.Id}&token={token}";

            var model = new ForgotPasswordViewDto
            {
                Url = confirmLink
            };
            var html = _viewRenderService.RenderToStringAsync("EmailTemplate/_forgotPasswordMail", model);

            await _emailService.SendEmailAsync(new SendEmailDto
            {
                To = query.Email,
                Message = html.Result,
                Subject = "Parola Yenileme"
            });
            return token;
        }

        public async Task<string> LoginAsync(UserSignInViewDto viewDto)
        {
            var userItem = await _context.Set<ApplicationUser>()
                                       .Where(x => x.UserName == viewDto.UserName)
                                       .Select(ApplicationUserProjectors.Project)
                                       .FirstOrDefaultAsync();
            ApplicationUser user = _mapper.Map<ApplicationUser>(userItem);
            if (user == null)
            {
                throw new ServiceException("Kullanıcı bulunamadı.");
            }
            if (!user.LockoutEnabled)
            {
                throw new ServiceException("Hesabınız kitlenmiştir.");
            }

            var identityResult = await _signInManager.PasswordSignInAsync(viewDto.UserName, viewDto.Password, viewDto.RememberMe, false);
            if (identityResult.IsLockedOut)
            {
                var incoming = await _userManager.GetLockoutEndDateAsync(await _userManager.FindByNameAsync(viewDto.UserName));

                var restrictedTime = incoming.Value;

                var remainingMinutes = restrictedTime.Minute - DateTime.Now.Minute;

                throw new ServiceException("3 kere yanlış girdiğiniz için hesabınız " + remainingMinutes + " dk kilitlenmiştir ");
            }
            if (identityResult.IsNotAllowed)
            {
                throw new ServiceException("Email adresinizi lütfen doğrulayınız.");
            }
            if (identityResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.CreateToken(user, roles);

                return token.Token;
            }
            return null;
        }

        public async Task<IdentityResult> ResetPasswordAsync(GetAccountResetPasswordQuery query)
        {
            var user = await _userManager.FindByIdAsync(query.Id.ToString());

            if (user == null)
            {
                throw new ServiceException("Kullanıcı bulunamadı.");
            }

            var result = await _userManager.ResetPasswordAsync(user, query.Token.Base64ForUrlDecode(), query.Password);


            return result;
        }
    }
}
