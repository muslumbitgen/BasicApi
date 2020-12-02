using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using BasicApi.Items.Options;
using BasicApi.Items.Types;
using System;
using System.Linq;

namespace BasicApi.Common.Mvc.Attributes
{
    public class PagingActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IOptions<PagingOptions> pagingOptions;

        public PagingActionFilterAttribute(IOptions<PagingOptions> options)
        {
            pagingOptions = options;
        }
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ActionArguments.Any() == false ||
                actionContext.ActionArguments.Any(x => x.Key == null) == true ||
                actionContext.ActionArguments.Any(x => x.Value == null) == true ||
                actionContext.ActionArguments.Any(x => x.Value.GetType().IsSubclassOf(typeof(PagedQueryBase))) == false)
            {
                return;
            }

            HeaderValueProvider provider = new HeaderValueProvider(actionContext.HttpContext);

            var page = provider.TryGetValue("X-Paging-Page") == string.Empty ? pagingOptions.Value.Page : Convert.ToInt32(provider.TryGetValue("X-Paging-Page"));
            var pageSize = provider.TryGetValue("X-Paging-PageSize") == string.Empty ? pagingOptions.Value.PageSize : Convert.ToInt32(provider.TryGetValue("X-Paging-PageSize"));
            var orderBy = provider.TryGetValue("X-Paging-OrderBy") == string.Empty ? pagingOptions.Value.OrderBy : provider.TryGetValue("X-Paging-OrderBy");
            var asc = provider.TryGetValue("X-Paging-Asc") == string.Empty ? pagingOptions.Value.Asc : Convert.ToBoolean(provider.TryGetValue("X-Paging-Asc"));

            var model = (PagedQueryBase)actionContext.ActionArguments.Where(x => x.Value.GetType().IsSubclassOf(typeof(PagedQueryBase))).FirstOrDefault().Value;

            model.Page = page;
            model.PageSize = pageSize;
            model.OrderBy = orderBy;
            model.Asc = asc;
        }
    }
}
