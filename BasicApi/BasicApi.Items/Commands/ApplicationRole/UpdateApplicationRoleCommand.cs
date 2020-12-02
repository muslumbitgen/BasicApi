using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BasicApi.Items.Commands
{
    public class UpdateApplicationRoleCommand : ICommand
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
