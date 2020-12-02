using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Dtos
{
    public class UserSignInViewDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
