
using Volo.Abp.Application.Dtos;

public class PagedInput : IPagedAndSortedResultRequest
{
    public int PageNum { get; set; }
    public int PageSize { get; set; }
    public int MaxResultCount
    {
        get { return PageSize; }
        set { }
    }

    public int SkipCount
    {
        get { return (PageNum -1) * PageSize ; }
        set { }
    }

    public string Sorting { get; set; }
}