using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ProjectUmico.Application.Common;

public class PaginatedList<T>
{
    private int _pageSize;
    private int _pageNumber;

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
                > 50 => 50,
                <= 0 => 1,
                _ => value
            };
        }
    }

    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public List<T> Items { get; set; }

    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;

    public PaginatedList(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int) Math.Ceiling((double) (TotalCount / PageSize));
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pagesize)
    {
        var count = await source.CountAsync();

        var result = await source.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();

        return new PaginatedList<T>(result, page, pagesize, count);
    }
}

public static class PaginatedListExtensions
{
    public static Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageSize,
        int pageNumber) where T : class
    {
        return PaginatedList<T>.CreateAsync(source.AsNoTracking(), pageNumber, pageSize);
    }
    
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}