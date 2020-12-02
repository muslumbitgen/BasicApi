using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BasicApi.Items;
using BasicApi.Items.Entities;
using BasicApi.Items.Options;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace BasicApi.Core.Services
{
    public class TokenService : ServiceBase, ITokenService
    {
        private DateTime _accessTokenExpiration;
        private IOptions<TokenOptions> _options { get; }

        public TokenService(IOptions<TokenOptions> options)
        {
            _options = options;
            _accessTokenExpiration = DateTime.Now.AddMinutes(_options.Value.AccessTokenExpiration);

        }
        public AccessToken CreateToken(ApplicationUser user, IList<string> applicationRoles)
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_options.Value.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(user, signingCredentials, applicationRoles);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }
        public JwtSecurityToken CreateJwtSecurityToken(ApplicationUser user,
        SigningCredentials signingCredentials, IList<string> applicationRoles)
        {
            var jwt = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, applicationRoles),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        private IEnumerable<Claim> SetClaims(ApplicationUser user, IList<string> applicationRoles)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(applicationRoles.ToArray());

            return claims;
        }
    }
}
