using BasicApi.Items.Dtos;
using BasicApi.Items.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BasicApi.Items.Projectors
{
    public class FeedbackProjectors
    {
        public static Expression<Func<Feedback, FeedbackViewDto>> Project
        {
            get
            {
                return x => new FeedbackViewDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Email = x.Email,
                    Subject = x.Subject,
                    Message = x.Message,
                    CreatedAt = x.CreatedAt
                };
            }
        }
    }
}
