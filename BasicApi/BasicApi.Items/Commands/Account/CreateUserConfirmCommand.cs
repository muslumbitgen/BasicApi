using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Commands
{
    public class CreateUserConfirmCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
    }
}
