using BasicApi.Items.Entities;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Core.Services
{
    public interface ITokenService
    {
        AccessToken CreateToken(ApplicationUser user, IList<string> applicationRoles);
    }
}
