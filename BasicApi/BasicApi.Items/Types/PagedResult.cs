using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicApi.Items.Types
{
    public class PagedResult<T> : PagedResultBase
    {
        public IEnumerable<T> Items { get; }

        protected PagedResult()
        {
            Items = Enumerable.Empty<T>();
        }

        protected PagedResult(IEnumerable<T> items, int page, int pageSize, int totalPages, long totalResults) : base(page, pageSize, totalPages, totalResults)
        {
            Items = items;
        }

        public static PagedResult<T> Create(IEnumerable<T> items, int page, int pageSize, int totalPages, long totalResults)
        {
            return new PagedResult<T>(items, page, pageSize, totalPages, totalResults);
        }

        public static PagedResult<T> Create(IEnumerable<T> items)
        {
            return new PagedResult<T>(items, 1, items.Count(), 1, items.Count());
        }

        public static PagedResult<T> From(PagedResultBase result, IEnumerable<T> items)
        {
            return new PagedResult<T>(items, result.Page, result.PageSize, result.TotalPages, result.TotalResults);
        }

        public static PagedResult<T> Empty => new PagedResult<T>();
    }
}
