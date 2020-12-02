using BasicApi.Items.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Commands
{
    public class CreateFeedbackCommand : ICommand
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
