using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Dtos
{
    public class FeedbackViewDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
