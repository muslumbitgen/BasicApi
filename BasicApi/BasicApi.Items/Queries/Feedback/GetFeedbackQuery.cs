using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Queries
{
    public class GetFeedbackQuery : IQuery
    {
        public Guid Id { get; set; }

    }
}
