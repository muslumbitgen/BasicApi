using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Commands
{
    public class CreateToRoleCommand : ICommand
    {
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
