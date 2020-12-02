using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Dtos
{
    public class RoleAssignViewDto
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public bool Exists { get; set; }
    }
}
