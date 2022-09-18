using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Models.Attributes;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Commands;

public static class DeleteAttributeCommandV1
{
    public record DeleteAttributeCommand(int Id) :IRequest<Result<AttributeDto>>;
    
    public class DeleteAttributeCommandHandler : IRequestHandler<DeleteAttributeCommand,Result<AttributeDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public DeleteAttributeCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<AttributeDto>> Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
        {
            var attribute = await _dbContext.Attributes
                .SingleOrDefaultAsync(a => a.Id == request.Id,cancellationToken);

            if (attribute is null)
            {
                throw new NotFoundException(nameof(ProductAttribute),nameof(ProductAttribute.Id),request.Id);
            }
            _dbContext.Attributes.Remove(attribute);
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