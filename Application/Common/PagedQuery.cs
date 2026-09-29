namespace Application.Common;

public record PagedQuery
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagedQuery(int page, int pageSize)
    {
        Page = Math.Max(1, page);
        PageSize = Math.Max(1, pageSize);
    }

    public PagedQuery() : this(page: 1, pageSize: 10) { }
};
