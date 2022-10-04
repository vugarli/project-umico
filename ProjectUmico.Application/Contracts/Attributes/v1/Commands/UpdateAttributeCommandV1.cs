using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Exceptions;
using ProjectUmico.Domain.Models.Attributes;
using Attribute = System.Attribute;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Commands;

public static class UpdateAttributeCommandV1
{
    public record UpdateAttributeCommand() : IRequest<Result<AttributeDto>>
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public AttributeType AttributeType { get; set; }
        public int? ParentAttributeId { get; set; } = default;
    }
    
    public class UpdateAttributeCommandHandler : IRequestHandler<UpdateAttributeCommand,Result<AttributeDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateAttributeCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<AttributeDto>> Handle(UpdateAttributeCommand request, CancellationToken cancellationToken)
        {
            var attribute = await _dbContext.Attributes
                .SingleOrDefaultAsync(a => a.Id == request.Id,cancellationToken);

            if (attribute is null)
            {
                var ex =  new NotFoundException(nameof(Domain.Models.Attributes.ProductAttribute),nameof(Domain.Models.Attributes.ProductAttribute.Id));
                return Result<AttributeDto>.Failure(ex);
            }
            if (request.AttributeType is AttributeType.AttributeGroup && request.ParentAttributeId != null)
            {
                var ex = new AttributeExceptions.GroupAttributeCantHaveParentException();
                return Result<AttributeDto>.Failure(ex);
            }
            // TODO validation add then fix controller
            if (request.AttributeType is AttributeType.Attribute && request.ParentAttributeId != null)
            {
                var parentAttribute =
                    await _dbContext.Attributes.AsNoTracking().SingleOrDefaultAsync(a => a.Id == request.ParentAttributeId,
                        cancellationToken);

                if (parentAttribute is null)
                {
                    throw new NotFoundException(nameof(ProductAttribute), nameof(ProductAttribute.Id), request.ParentAttributeId);
                }

                if (parentAttribute.AttributeType is AttributeType.Attribute)
                {
                    throw new AttributeExceptions.AttributeCantHaveAttributeParentException();
                }
            }
            else if(request.AttributeType is AttributeType.Attribute)
            {
                throw new AttributeExceptions.AttributeMustHaveParentException(); 
            }
            
            _mapper.Map<UpdateAttributeCommand,ProductAttribute>(request,attribute);
            
            var result = await _dbContext.SaveChangesAsync(cancellationToken);            
            
            if (result > 0)
            {
                var attributeDto = _mapper.Map<AttributeDto>(attribute);
                return Result<AttributeDto>.Success(attributeDto);
            }
            else return Result<AttributeDto>.Failure();
        }
    }
}