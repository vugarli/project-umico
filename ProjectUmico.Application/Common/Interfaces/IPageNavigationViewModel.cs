namespace ProjectUmico.Application.Common.Interfaces;

public interface IPageNavigationViewModel
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get;  }
    public bool HasPreviousPage { get; }
}