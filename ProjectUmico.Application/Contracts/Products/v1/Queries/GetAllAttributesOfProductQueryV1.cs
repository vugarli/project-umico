using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Contracts;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;

namespace ProjectUmico.Application.Products.Queries;

public static class GetAllAttributesOfProductQueryV1
{

    public record GetAllAttributesOfProductQuery(int productId,PaginationQuery query):IRequest<PaginatedList<AttributeDto>>;

    public class GetAllAttributesOfProductQueryHandler : IRequestHandler<GetAllAttributesOfProductQuery,PaginatedList<AttributeDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllAttributesOfProductQueryHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<PaginatedList<AttributeDto>> Handle(GetAllAttributesOfProductQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _dbContext.Products
                .Where(p=>p.Id == request.productId)
                .SelectMany(p=>p.Atributes)
                .ToPaginatedListWithProjectionAsync<ProductAttribute,AttributeDto>(request.query.PageSize,request.query.PageNumber,_mapper);

            return attributes;
        }
    }
}