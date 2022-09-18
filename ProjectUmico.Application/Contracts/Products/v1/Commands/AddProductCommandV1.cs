using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Application.Common.Models;
using ProjectUmico.Application.Dtos;
using ProjectUmico.Domain.Models.Attributes;
using umico.Models;
using umico.Models.Categories;

namespace ProjectUmico.Application.Products.Commands;

public static class AddProductCommandV1
{
    public record AddProductCommand(): IRequest<Result<ProductDto>>
    {
        public string Sku { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? ThumbnailUrl { get; set; }

        public List<int>? AttributeIds { get; set; } = new();
        
        public int CategoryId { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand,Result<ProductDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddProductCommandHandler(IApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Result<ProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Product();
        
            _mapper.Map(request,newProduct);

            var attributes = new List<ProductAttribute>();
            if (request.AttributeIds != null && request.AttributeIds.Any() )
            {
                foreach (var id in request.AttributeIds)
                {
                    var attribute = new ProductAttribute() {Id = id};
                    attributes.Add(attribute);
                }
                newProduct.Atributes = attributes;
            }

            var category = new Category()
            {
                Id = request.CategoryId
            };
            
            _dbContext.Products.Attach(newProduct);
            
            // TODO fetch product from db to show null fields
            
            var result= await _dbContext.SaveChangesAsync(cancellationToken);
        
            if (result > 0)
            {
                var productDto = _mapper.Map<ProductDto>(newProduct);
                return Result<ProductDto>.Success(productDto);
            }
            else return Result<ProductDto>.Failure();
        }
    }
}