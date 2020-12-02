using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BasicApi.Items.Commands
{
    public class CreateApplicationRoleCommand : ICommand
    {
        public string Name { get; set; }
    }
}
