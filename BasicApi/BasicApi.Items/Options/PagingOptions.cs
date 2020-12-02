namespace BasicApi.Items.Options
{
    public class PagingOptions
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string OrderBy { get; set; }

        public bool Asc { get; set; }
    }
}
