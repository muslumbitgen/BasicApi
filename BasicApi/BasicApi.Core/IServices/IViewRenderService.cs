using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasicApi.Core.Services
{
    public interface IViewRenderService : IServiceBase
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
