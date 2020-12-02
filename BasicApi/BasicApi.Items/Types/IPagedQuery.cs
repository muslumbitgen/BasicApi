using BasicApi.Items.Messages;

namespace BasicApi.Items.Types
{
    public interface IPagedQuery : IQuery
    {
        int Page { get; }

        int PageSize { get; }

        string OrderBy { get; }

        bool Asc { get; }
    }
}
