using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

namespace BasicApi.Common.Mvc
{
    public class HeaderValueProvider : IValueProvider
    {
        private readonly HttpContext context;
        public HeaderValueProvider(HttpContext httpContext)
        {
            context = httpContext;
        }
        public bool ContainsPrefix(string prefix)
        {
            return context.Request.Headers.ContainsKey(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            return context.Request.Headers.TryGetValue(key, out StringValues values)
                ? new ValueProviderResult(values)
                : new ValueProviderResult();
        }
        public string TryGetValue(string key)
        {
            if (GetValue(key).Length > 0)
            {
                return GetValue(key).FirstValue;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
