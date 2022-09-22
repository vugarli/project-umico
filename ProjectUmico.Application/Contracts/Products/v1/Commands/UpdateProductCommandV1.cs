using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;

namespace ProjectUmico.Application.Products.v1.Commands;

public static  class UpdateProductCommandV1
{
    public record UpdateProductCommand : IRequest<Result<ProductDto>>
    {
        // do not change names of properties. Mapper uses them 
        public int Id { get; set; } 
        public string SKU { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }
        public int CategoryId { get; set; }
    }
    
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand,Result<ProductDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IApplicationDbContext context,IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        
        public async Task<Result<ProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var oldProduct = await _dbContext.Products.SingleOrDefaultAsync(p=>p.Id == request.Id);

            if (oldProduct is null)
            {
                throw new NotFoundException(nameof(Product),nameof(Product.Id),request.Id);
            }
            
            // mapper only maps non null values. Null values on request won't be mapped to source.
            // Also for convenience params with "string" value won't be mapped as Swagger defaults to that value for string type.
            
            _mapper.Map(request,oldProduct);
            
            if (request.CategoryId != default)
            {
                oldProduct.CategoryId = request.CategoryId;
            }

            var result = await _dbContext.SaveChangesAsync(cancellationToken);
        
            if (result > 0)
            {
                var productDto = _mapper.Map<ProductDto>(oldProduct);
                return Result<ProductDto>.Success(productDto);
            }
            else return Result<ProductDto>.Failure();
        }
    }
}