using BasicApi.Items.Dtos;
using BasicApi.Items.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BasicApi.Items.Projectors
{
    public class ApplicationRoleProjectors
    {
        public static Expression<Func<ApplicationRole, ApplicationRoleViewDto>> Project
        {
            get
            {
                return x => new ApplicationRoleViewDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedAt = x.CreatedAt.ToString("dd.MM.yyyy")
                };
            }
        }
    }
}
