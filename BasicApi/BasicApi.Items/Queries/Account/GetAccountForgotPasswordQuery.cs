using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BasicApi.Items.Queries
{
    public class GetAccountForgotPasswordQuery : IQuery
    {
        public string Email { get; set; }
    }
}
