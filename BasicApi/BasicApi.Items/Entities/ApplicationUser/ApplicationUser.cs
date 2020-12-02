using Microsoft.AspNetCore.Identity;
using BasicApi.Items.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string About { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
