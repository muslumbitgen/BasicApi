using BasicApi.Items.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailDto sendEmail);

    }
}
