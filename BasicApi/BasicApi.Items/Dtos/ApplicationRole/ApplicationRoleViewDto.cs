using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Dtos
{
    public class ApplicationRoleViewDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string CreatedAt { get; set; }

    }
}
