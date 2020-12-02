using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Dtos
{
    public class ApplicationUserViewDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string About { get; set; }

        public bool LockoutEnabled { get; set; }

        public bool EmailConfirmed { get; set; }

        public IList<string> Roles { get; set; }

        public string CreatedAt { get; set; }

    }
}
