using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Commands
{
    public class UpdateApplicationUserActiveCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
