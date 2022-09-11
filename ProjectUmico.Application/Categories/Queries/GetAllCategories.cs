using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Dtos;
using umico.Models.Categories;

namespace ProjectUmico.Application.Categories.Queries;

public class GetAllCategories : IRequest<PaginatedList<CategoryDto>>
{
    public readonly PaginationQuery Query;

    public GetAllCategories(PaginationQuery query)
    {
        Query = query;
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
            .ToPaginatedListAsync(request.Query.PageSize,request.Query.PageNumber);

        return categories;
    }
}

