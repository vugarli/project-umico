using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using Attribute = ProjectUmico.Domain.Models.Attributes.Attribute;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Queries;

public static class GetAttributeByIdQueryV1
{
    public record GetAttributeByIdQuery(int Id) : IRequest<AttributeDto>;
    
    
    public class GetAttributeByIdQueryHandler : IRequestHandler<GetAttributeByIdQuery,AttributeDto>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAttributeByIdQueryHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<AttributeDto> Handle(GetAttributeByIdQuery request, CancellationToken cancellationToken)
        {
            var attribute = await _dbContext.Attributes.SingleOrDefaultAsync(a=>a.Id == request.Id,cancellationToken);

            if (attribute is null)
            {
                throw new NotFoundException(nameof(Attribute),nameof(Attribute.Id),request.Id);
            }

            return _mapper.Map<AttributeDto>(attribute);
        }
    }
}