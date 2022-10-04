using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Queries;

public class GetAllAttributesQueryV1
{
    public record GetAllAttributesQuery(PaginationQuery Query) : IRequest<Result<PaginatedList<AttributeDto>>>;


    public class GetAllAttributesQueryHandler : IRequestHandler<GetAllAttributesQuery, 
        Result<PaginatedList<AttributeDto>>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllAttributesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<AttributeDto>>> Handle(GetAllAttributesQuery request, CancellationToken cancellationToken)
        {
            var attributes = await _dbContext.Attributes.ProjectTo<AttributeDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Query.PageSize,request.Query.PageNumber);
            
            return Result<PaginatedList<AttributeDto>>.Success(attributes);
        }
    }
}