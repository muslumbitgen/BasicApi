using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Queries
{
    public class GetApplicationUserEmailQuery : IQuery
    {
        public string Email { get; set; }
    }
}
