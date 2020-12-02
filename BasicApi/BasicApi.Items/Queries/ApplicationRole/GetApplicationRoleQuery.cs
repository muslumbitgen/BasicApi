using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Queries
{
    public class GetApplicationRoleQuery : IQuery
    {
        public Guid Id { get; set; }
    }
}
