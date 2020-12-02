namespace BasicApi.Items.Types
{
    public abstract class PagedQueryBase : IPagedQuery
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string OrderBy { get; set; }

        public bool Asc { get; set; }
    }
}
