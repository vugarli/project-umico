using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Exceptions;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using umico.Models;

namespace ProjectUmico.Application.Products.v1.Commands;

public static  class UpdateProductCommandV1
{
    public record UpdateProductCommand : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }
        public string Sku { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }

        public List<int>? AttributeIds { get; set; } = new();
        
        public int CategoryId { get; set; }
    }
    
    public class AddProductCommandHandler : IRequestHandler<UpdateProductCommand,Result<ProductDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IApplicationDbContext context,IMapper mapper)
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
            // TODO update this part with mapper
            oldProduct.Name = request.Name;
            oldProduct.SKU = request.Sku;
            
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