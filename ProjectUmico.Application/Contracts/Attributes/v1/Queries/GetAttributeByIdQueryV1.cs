using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Models.Attributes;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Queries;

public static class GetAttributeByIdQueryV1
{
    public record GetAttributeByIdQuery(int Id) : IRequest<Result<AttributeDto>>;
    
    
    public class GetAttributeByIdQueryHandler : IRequestHandler<GetAttributeByIdQuery,Result<AttributeDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAttributeByIdQueryHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        public async Task<Result<AttributeDto>> Handle(GetAttributeByIdQuery request, CancellationToken cancellationToken)
        {
            var attribute = await _dbContext.Attributes.SingleOrDefaultAsync(a=>a.Id == request.Id,cancellationToken);

            if (attribute is null)
            {
                var exception = 
                    new NotFoundException(nameof(ProductAttribute),
                        nameof(ProductAttribute.Id),request.Id);
                Result<AttributeDto>.Failure(exception);
            }

            var attributeDto =  _mapper.Map<AttributeDto>(attribute);

            return Result<AttributeDto>.Success(attributeDto);
        }
    }
}