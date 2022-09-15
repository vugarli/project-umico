using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Application.Products.Queries;

public class GetAllProductsQuery : IRequest<PaginatedList<ProductDto>>
{
    public PaginationQuery Query;

    public GetAllProductsQuery(PaginationQuery query)
    {
        Query = query;
    }
}

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery,PaginatedList<ProductDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IApplicationDbContext dbContext,IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<PaginatedList<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _dbContext.Products.AsNoTracking()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Query.PageSize, request.Query.PageNumber);

        return products;
    }
}