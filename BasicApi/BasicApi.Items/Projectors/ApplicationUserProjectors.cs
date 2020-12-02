using BasicApi.Items.Dtos;
using BasicApi.Items.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BasicApi.Items.Projectors
{
    public class ApplicationUserProjectors
    {
        public static Expression<Func<ApplicationUser, ApplicationUserViewDto>> Project
        {
            get
            {
                return x => new ApplicationUserViewDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    About = x.About,
                    LockoutEnabled = x.LockoutEnabled,
                    EmailConfirmed = x.EmailConfirmed,
                    CreatedAt = x.CreatedAt.ToString("dd.MM.yyyy")
                };
            }
        }
    }
}
