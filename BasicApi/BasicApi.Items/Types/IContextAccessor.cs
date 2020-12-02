using Microsoft.AspNetCore.Http;
using System;

namespace BasicApi.Items.Types
{
    public interface IContextAccessor : IHttpContextAccessor
    {
        Guid UserId { get; }
    }
}
