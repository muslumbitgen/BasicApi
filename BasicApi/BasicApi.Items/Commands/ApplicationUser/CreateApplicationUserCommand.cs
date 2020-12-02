using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BasicApi.Items.Commands
{
    public class CreateApplicationUserCommand : ICommand
    {
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string About { get; set; }
    }
}
