using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Interfaces;

namespace ProjectUmico.Application.Common;

public class PaginatedList<T> : IPageNavigationViewModel
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
                > 50 => PaginatedListExtensions.MaxPageSize,
                <= 0 => PaginatedListExtensions.MinPageSize,
                _ => value
            };
        }
    }

    public int TotalPages { get; set; }
    public int TotalCount { get; set; } // total count of items 

    public List<T> Items { get; set; }

    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;

    public PaginatedList(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int) Math.Ceiling((TotalCount / (double)PageSize));
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int page, int pagesize)
    {
        var count = await source.CountAsync();

        var result = await source.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();

        return new PaginatedList<T>(result, page, pagesize, count);
    }
    public static async Task<PaginatedList<R>> CreateWithProjectionAsync<T,R>(IQueryable<T> source, int page, int pagesize,IMapper mapper)
    where R : class
    {
        var count = await source.CountAsync();

        var result = await source.Skip((page - 1) * pagesize).Take(pagesize)
            .ProjectToListAsync<R>(mapper.ConfigurationProvider);

        return new PaginatedList<R>(result, page, pagesize, count);
    }
}

public static class PaginatedListExtensions
{
    public static readonly int MinPageSize = 10;
    public static readonly int MaxPageSize = 50;
    
    public static Task<PaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageSize,
        int pageNumber) where T : class
    {
        return PaginatedList<T>.CreateAsync(source.AsNoTracking(), pageNumber, pageSize);
    }
    public static Task<PaginatedList<R>> ToPaginatedListWithProjectionAsync<T,R>(this IQueryable<T> source, int pageSize,
        int pageNumber,IMapper mapper) 
        where T : class
        where R : class
    {
        return PaginatedList<T>.CreateWithProjectionAsync<T,R>(source.AsNoTracking(), pageNumber, pageSize,mapper);
    }
    
    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
}