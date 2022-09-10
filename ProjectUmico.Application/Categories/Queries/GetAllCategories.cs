using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.Queries;

public class GetAllCategories : IRequest<PaginatedList<CategoryDto>>
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
                <= 0 => 10,
                _ => value
            };
        }
    }

    public GetAllCategories(int pageNumber,int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategories,PaginatedList<CategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<CategoryDto>> Handle(GetAllCategories request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories.Include(c => c.Parent)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageSize,request.PageNumber);

        return categories;
    }
}

