using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Exceptions;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Commands;

public static class AddAttributeCommandV1
{
    public record AddAttributeCommand : IRequest<Result<AttributeDto>>
    {
        public string Value { get; set; }
        public AttributeType AttributeType { get; set; }
        public int? ParentAttributeId { get; set; } = default;
    }

    public class AddAttributeCommandHandler : IRequestHandler<AddAttributeCommand, Result<AttributeDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddAttributeCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<AttributeDto>> Handle(AddAttributeCommand request, CancellationToken cancellationToken)
        {
            var attribute = _mapper.Map<ProductAttribute>(request);
            
            _dbContext.Attributes.Add(attribute);

            int result = await _dbContext.SaveChangesAsync(cancellationToken);
            
            
            if (result > 0)
            {
                var attributeDto = _mapper.Map<AttributeDto>(attribute);
                return Result<AttributeDto>.Success(attributeDto);
            }
            else return Result<AttributeDto>.Failure();
        }
    }
}