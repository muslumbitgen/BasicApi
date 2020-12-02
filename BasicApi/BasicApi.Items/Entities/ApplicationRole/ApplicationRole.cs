using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
