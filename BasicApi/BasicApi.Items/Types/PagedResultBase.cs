using System;
using System.Collections.Generic;
using System.Text;

namespace BasicApi.Items.Types
{
    public abstract class PagedResultBase
    {
        public int Page { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public long TotalResults { get; }

        protected PagedResultBase()
        {
        }

        protected PagedResultBase(int page, int pageSize, int totalPages, long totalResults)
        {
            Page = page > totalPages ? totalPages : page;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalResults = totalResults;
        }
    }
}
