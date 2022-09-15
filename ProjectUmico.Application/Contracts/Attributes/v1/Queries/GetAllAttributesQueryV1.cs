using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using ProjectUmico.Application.Common;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Queries;

public class GetAllAttributesQueryV1
{
    public record GetAllAttributesQuery(PaginationQuery Query) : IRequest<PaginatedList<AttributeDto>>;


    public class GetAllAttributesQueryHandler : IRequestHandler<GetAllAttributesQuery, PaginatedList<AttributeDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllAttributesQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<PaginatedList<AttributeDto>> Handle(GetAllAttributesQuery request, CancellationToken cancellationToken)
        {
            var attributes = _dbContext.Attributes.ProjectTo<AttributeDto>(_mapper.ConfigurationProvider)
                .ToPaginatedListAsync(request.Query.PageSize,request.Query.PageNumber);
            return attributes;
        }
    }
}