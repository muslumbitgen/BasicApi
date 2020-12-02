using BasicApi.Items.Dtos;
using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Commands
{
    public class CreateAssignRoleCommand : ICommand
    {
        public Guid UserId { get; set; }

        public IList<RoleAssignViewDto> UserRoles { get; set; }

    }
}
