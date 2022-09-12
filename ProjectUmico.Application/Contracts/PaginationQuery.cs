using System.Reflection.Metadata.Ecma335;
using ProjectUmico.Application.Common;

namespace ProjectUmico.Application.Contracts;

public class PaginationQuery
{
    private int _pageSize = PaginatedListExtensions.MinPageSize;
    private int _pageNumber = 1;
    
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSize = value switch
            {
                > 50 => PaginatedListExtensions.MaxPageSize,
                <= 0 => PaginatedListExtensions.MinPageSize,
                _ => value
            };
        }
    }
    
    
    public PaginationQuery(int pageNumber,int pageSize,string actionName,string controllerName)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public PaginationQuery() { }
}