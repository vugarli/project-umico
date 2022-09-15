using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;

namespace ProjectUmico.Application.Contracts.Attributes.v1.Commands;

public static class UpdateAttributeCommandV1
{
    public record UpdateAttributeCommand() : IRequest<Result<AttributeDto>>
    {
        public int Id { get; set; }
        public string Value { get; set; } = default!;
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
            var attribute = await _dbContext.Attributes.SingleOrDefaultAsync(a => a.Id == request.Id,cancellationToken);

            if (attribute is null)
            {
                throw new NotFoundException(nameof(Domain.Models.Attributes.Attribute),nameof(Domain.Models.Attributes.Attribute.Id));
            }

            attribute.Value = request.Value;
            
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