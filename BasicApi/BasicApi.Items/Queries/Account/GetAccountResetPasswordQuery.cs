using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Queries
{
    public class GetAccountResetPasswordQuery : IQuery
    {
        public string Token { get; set; }

        public Guid Id { get; set; }

        public string Password { get; set; }
    }
}
